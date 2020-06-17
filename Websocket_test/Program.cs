using Fleck;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Websocket_test
{
	class Program
	{
		static void Main(string[] args)
		{
			FleckLog.Level = LogLevel.Debug;

			var allSockets = new List<IWebSocketConnection>();

			var server = new WebSocketServer("ws://192.168.11.200:7181");

			//websocket 启动
			server.Start(socket => {
				//开启连接
				socket.OnOpen = () => {
					Console.WriteLine("开启服务");
					allSockets.Add(socket);
				};

				//关闭连接
				socket.OnClose = () => {
					Console.WriteLine("关闭服务");
					allSockets.Remove(socket);
				};

				//接受客户端信息
				socket.OnMessage = message =>
				{
					Console.WriteLine(message);
					allSockets.ToList().ForEach(s => s.Send("Client: " + message));
				};
			});

			//给客户端发送信息
			var input = Console.ReadLine();
			while (input != "exit")
			{
				foreach (var socket in allSockets.ToList())
				{
					socket.Send(input);
				}
				input = Console.ReadLine();
			}
		}
	}
}
