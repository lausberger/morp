import java.io.IOException;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.util.Scanner;

public class Client {

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
  
  public static void main(String args[]) throws IOException {
    Scanner scanner = new Scanner(System.in);

    DatagramSocket socket = new DatagramSocket();

    InetAddress ip = InetAddress.getLocalHost();
    byte buffer[] = null;

    // debug
    DatagramPacket response = null;
    byte[] receive = new byte[65535];

    while (true) {
      if (scanner.hasNextLine()) {
        String input = scanner.nextLine();
        buffer = input.getBytes();

        DatagramPacket packet = new DatagramPacket(buffer, buffer.length, ip, 3390);
        socket.send(packet);
      
        if (input.equals("END")) break;
      } 

      if (data(receive).toString().equals("END")) {
        System.out.println("Received END from server. Exiting");
        break;
      }

      // debug
      response = new DatagramPacket(receive, receive.length);
      socket.receive(response);
      System.out.println("Server: " + data(receive));

      receive = new byte[65535];
    }

    scanner.close();
    socket.close();
  }
}