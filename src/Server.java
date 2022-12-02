import java.io.IOException;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.util.Scanner;

import javax.print.event.PrintEvent;

public class Server {

  public static StringBuilder data(byte[] a) {
    if (a == null) return null;
    StringBuilder data = new StringBuilder();
    int i = 0;
    while (a[i] != 0) {
      data.append((char) a[i]);
      i++;
    }
    return data;
  }

  public static void main(String[] args) throws IOException {
    Scanner scanner = new Scanner(System.in);

    DatagramSocket socket = new DatagramSocket(3390);
    byte[] receive = new byte[65535];

    DatagramPacket packet = null;

    // debug
    String msg = "PONG";
    byte[] response = msg.getBytes();

    while (true) {
      packet = new DatagramPacket(receive, receive.length);
      socket.receive(packet);

      System.out.println("Client: " + data(receive));

      if (data(receive).toString().equals("END")) {
        System.out.println("Received END from client. Exiting");
        break;
      }

      // debug
      InetAddress ip = packet.getAddress();
      int port = packet.getPort();

      // allow server to respond manually
      if (scanner.hasNextLine()) {
        String input = scanner.nextLine();
        response = input.getBytes();
      } else {
        System.out.println("Nothing to send");
      }
      packet = new DatagramPacket(response, response.length, ip, port);
      socket.send(packet);

      receive = new byte[65535];
    }

    socket.close();
    scanner.close();
  }
}