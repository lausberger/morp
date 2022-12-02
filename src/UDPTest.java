import java.io.IOException;
import java.net.SocketException;
import java.net.UnknownHostException;

public class UDPTest {
  AltClient client;

  public void setup() throws SocketException, UnknownHostException {
      new AltServer().start();
      client = new AltClient();
  }

  public void sendAndReceive() throws IOException {
      String message = "hello";
      String echo = client.sendPacket(message);
      System.out.println("echo == ping: " + echo.equals("ping"));
      echo = client.sendPacket("ping");
      System.out.println("echo == ping: " + echo.equals("ping"));
  }

  public void tearDown() throws IOException {
      client.sendPacket("end");
      client.close();
  }

  public static void main(String[] args) throws IOException {
    UDPTest guy = new UDPTest();
    guy.setup();
    guy.sendAndReceive();
    guy.tearDown();
  }
}