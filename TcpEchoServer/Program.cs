using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using ClassLibraryNET;
using System.Text.Json;
using WebApplicationClass;
using WebApplicationClass.Managers;
using Microsoft.EntityFrameworkCore;

namespace TcpEchoServer
{
    class Program
    {
        private static List<Book> _books = new List<Book>()
        {
            new Book("Bella", "Gugo", 23, 1),
            new Book("Pedro", "Pedritto", 50, 2),
            new Book("Don Juan La Della Coca", "Pelmen", 345, 3),
        };
        

        static void Main()
        {
            Console.WriteLine("This is the Book server");
            TcpListener listener = new TcpListener(IPAddress.Loopback, 4646);
            //DbContextOptions dbContextOptions = new DbContextOptions();
            BookManager manager = new BookManager(new AppDbContext(new DbContextOptions<AppDbContext>()));
            listener.Start();
            while (true)
            {
                Console.WriteLine("Server ready");
                TcpClient socket = listener.AcceptTcpClient();
                Console.WriteLine("Incoming client");
                Task.Run(() =>
                {
                    HandleClient(socket);
                });

                
            }
        }

        private static void HandleClient(TcpClient socket)
        {
            NetworkStream ns = socket.GetStream();
            StreamReader reader = new StreamReader(ns);
            StreamWriter writer = new StreamWriter(ns);
            string message = reader.ReadLine();

            BookManager manager = new BookManager(new AppDbContext(new DbContextOptions<AppDbContext>()));

            if (message == "Exit") ;


            Book book = JsonSerializer.Deserialize<Book>(message);

            Console.WriteLine("Server received the book: " + message);

            //switch (message)
            //{
            //    case "GetAll":
            //        var listOfBooks = manager.GetAll();
            //        foreach (var item in listOfBooks)
            //        {
            //            writer.WriteLine(item.Title);
            //            writer.WriteLine(book.Author);
            //            writer.WriteLine(book.BookId);
            //            writer.WriteLine(book.ISBN13);
            //            writer.WriteLine(book.PageNumber);
            //        }

            //        break;
            //    //case "Get":

            //}

            writer.Write("Book received");
            writer.Flush();
            socket.Close();
        }

    }
}
