using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OrderService.Data;
using OrderService.Dtos;
using OrderService.Helpers;
using OrderService.Models;

namespace OrderService.KafkaHandler
{
    public class MessageConsumer
    {
        public MessageConsumer()
        {
        }

        public static async Task<int> Consume(bool isProd)
        {
            string jsonConf = String.Empty;
            if(isProd) jsonConf = "appsettings.Production.json";
            else jsonConf = "appsettings.Development.json";
            
            Console.WriteLine(jsonConf);

            var builder = new ConfigurationBuilder()
                    .AddJsonFile(jsonConf, true, true);

            var config = builder.Build();
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = config["Settings:KafkaServer"],
                ClientId = Dns.GetHostName(),
            };
            var topics = new List<String>();
            topics.Add("order-add");
            topics.Add("user-add");
            foreach(var topic in topics)
            {
                using (var adminClient = new AdminClientBuilder(producerConfig).Build())
                {
                    Console.WriteLine("Creating a topic....");
                    try
                    {
                        await adminClient.CreateTopicsAsync(new List<TopicSpecification> {
                        new TopicSpecification { Name = topic, NumPartitions = 1, ReplicationFactor = 1 } });
                    }
                    catch (CreateTopicsException e)
                    {
                        if (e.Results[0].Error.Code != ErrorCode.TopicAlreadyExists)
                        {
                            Console.WriteLine($"An error occured creating topic {topic}: {e.Results[0].Error.Reason}");
                        }
                        else
                        {
                            Console.WriteLine("Topic already exists");
                        }
                    }
                }
            }
            
            var Serverconfig = new ConsumerConfig
            {
                BootstrapServers = config["Settings:KafkaServer"],
                GroupId = "orders-2",
                AutoOffsetReset = AutoOffsetReset.Latest
            };
            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) => {
                e.Cancel = true; // prevent the process from terminating.
                cts.Cancel();
            };
            Console.WriteLine("--------------.NET Application--------------");
            using (var consumer = new ConsumerBuilder<string, string>(Serverconfig).Build())
            {
                consumer.Subscribe(topics);

                Console.WriteLine("Waiting messages....");
                try
                {
                    while (true)
                    {
                        var cr = consumer.Consume(cts.Token);
                        Console.WriteLine($"Consumed record with Topic: {cr.Topic} key: {cr.Message.Key} and value: {cr.Message.Value}");

                        using (var dbcontext = new AppDbContext())
                        {
                            switch(cr.Topic)
                            {
                                case "order-add":
                                Console.WriteLine(cr.Topic);
                                OrderDto orderDto = JsonConvert.DeserializeObject<OrderDto>(cr.Message.Value);
                                Order order = new Order
                                {
                                    CustomerId = orderDto.CustomerId,
                                    UserLatitude = orderDto.UserLatitude,
                                    UserLongitude = orderDto.UserLongitude,
                                    Distance = orderDto.Distance,
                                    Price = orderDto.Price,
                                    PickedUp = orderDto.PickedUp,
                                    Completed = orderDto.Completed
                                };
                                dbcontext.Orders.Add(order);
                                break;
                                case "user-add":
                                Console.WriteLine(cr.Topic);
                                Customer customer = JsonConvert.DeserializeObject<Customer>(cr.Message.Value);
                                Customer newCustomer = new Customer
                                {
                                    Username = customer.Username,
                                    FirstName = customer.FirstName,
                                    LastName = customer.LastName,
                                    PhoneNumber = customer.PhoneNumber,
                                    Email = customer.Email,
                                    Balance = customer.Balance,
                                    CreatedDate = customer.CreatedDate,
                                    Blocked = customer.Blocked
                                };
                                dbcontext.Customers.Add(newCustomer);
                                break;
                            }
                            dbcontext.SaveChanges();
                            Console.WriteLine("Data was saved into database");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                }
                finally
                {
                    consumer.Close();
                }

            }
            return 1;
        }
    }
}