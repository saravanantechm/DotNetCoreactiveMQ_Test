using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DotnetCoreMVCActiveQ.Models;
using Apache.NMS;
using Apache.NMS.Util;

namespace DotnetCoreMVCActiveQ.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            Uri connecturi = new Uri("activemq:tcp://localhost:61616");

            Console.WriteLine("About to connect to " + connecturi);

            // NOTE: ensure the nmsprovider-activemq.config file exists in the executable folder.
            IConnectionFactory factory = new NMSConnectionFactory(connecturi);

            using (IConnection connection = factory.CreateConnection())
            using (ISession session = connection.CreateSession())
            {

                IDestination destination = SessionUtil.GetDestination(session, "queue://TECHM.BAR");
                Console.WriteLine("Using destination: " + destination);

                // Create a consumer and producer
                using (IMessageConsumer consumer = session.CreateConsumer(destination))
                using (IMessageProducer producer = session.CreateProducer(destination))
                {
                    // Start the connection so that messages will be processed.
                    connection.Start();
                    producer.DeliveryMode = MsgDeliveryMode.Persistent;

                    // Send a message
                    // ITextMessage request = session.CreateTextMessage("Hello World!");
                    var ss = "Test Message " + DateTime.Now;
                    ITextMessage request = session.CreateTextMessage(ss);
                    request.NMSCorrelationID = "abc";
                    request.Properties["NMSXGroupID"] = "cheese";
                    request.Properties["myHeader"] = "Cheddar";

                      producer.Send(request);

                    //lblmsg.Text = "Message from the Active MQ is: <b>" + request.Text + "</b>";

                    // Consume a message
                     // ITextMessage message = consumer.Receive() as ITextMessage;
                    // if (message == null)
                    //  {
                    //      Console.WriteLine("No message received!");
                   //   }
                   //   else
                    //  {
                        // Console.WriteLine("Received message with ID:   " + message.NMSMessageId);
                        //Console.WriteLine("Received message with text: " + message.Text);
                        //TempData["notice"] = "The Queue Message is:" + request.Text;
                    //    TempData["notice"] = "The Queue Message ID:" + message.NMSMessageId + " Message:" + message.Text;
                        //return RedirectToAction("Index");
                   // }
                }
            }
            return View();
            
        }

        public IActionResult About()
        {
            // ViewData["Message"] = "Your application description page.";

            Uri connecturi = new Uri("activemq:tcp://localhost:61616");

            Console.WriteLine("About to connect to " + connecturi);

            // NOTE: ensure the nmsprovider-activemq.config file exists in the executable folder.
            IConnectionFactory factory = new NMSConnectionFactory(connecturi);

            using (IConnection connection = factory.CreateConnection())
            using (ISession session = connection.CreateSession())
            {

                IDestination destination = SessionUtil.GetDestination(session, "queue://TECHM.BAR");
                Console.WriteLine("Using destination: " + destination);

                // Create a consumer and producer
                using (IMessageConsumer consumer = session.CreateConsumer(destination))
                using (IMessageProducer producer = session.CreateProducer(destination))
                {
                    // Start the connection so that messages will be processed.
                    connection.Start();
                    producer.DeliveryMode = MsgDeliveryMode.Persistent;

                    // Send a message
                    // ITextMessage request = session.CreateTextMessage("Hello World!");
                    var ss = "Test Message " + DateTime.Now;
                    ITextMessage request = session.CreateTextMessage(ss);
                    request.NMSCorrelationID = "abc";
                    request.Properties["NMSXGroupID"] = "cheese";
                    request.Properties["myHeader"] = "Cheddar";

                  //  producer.Send(request);

                    //lblmsg.Text = "Message from the Active MQ is: <b>" + request.Text + "</b>";

                    // Consume a message
                    ITextMessage message = consumer.Receive() as ITextMessage;
                    if (message == null)
                    {
                        Console.WriteLine("No message received!");
                    }
                    else
                    {
                        // Console.WriteLine("Received message with ID:   " + message.NMSMessageId);
                        //Console.WriteLine("Received message with text: " + message.Text);
                        //TempData["notice"] = "The Queue Message is:" + request.Text;
                        TempData["notice"] = "The Queue Message ID:" + message.NMSMessageId + " Message:" + message.Text;
                        //return RedirectToAction("Index");
                    }
                }
            }
          

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
