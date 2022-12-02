import java.io.IOException;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.net.SocketException;
import java.net.UnknownHostException;

public class AltClient {
  private DatagramSocket socket;
  private InetAddress address;
  private byte[] buf;

  public AltClient() throws SocketException, UnknownHostException {
    socket = new DatagramSocket();
    address = InetAddress.getByName("localhost");
  }

  public void logPacket(String received) {
    System.out.println("Client: " + received);
  }

  public String sendPacket(String msg) throws IOException {
    buf = msg.getBytes();
    var packet = new DatagramPacket(buf, buf.length, address, 4445);
    socket.send(packet);

    packet = new DatagramPacket(buf, buf.length);
    socket.receive(packet);

    String received = new String(packet.getData(), 0, packet.getLength());
    return received;
  }

  public void close() {
    socket.close();
  }
}