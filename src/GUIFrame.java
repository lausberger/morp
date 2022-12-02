import javax.swing.JFrame;
import javax.swing.JLabel;
import java.awt.Color;
import java.awt.event.*;

public class GUIFrame extends JFrame implements KeyListener {
  
  JLabel label;
  int speed;

  GUIFrame(int movementSpeed) {
    this.speed = movementSpeed;

    this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
    this.setSize(500, 500);
    this.setLayout(null);
    this.addKeyListener(this);

    label = new JLabel();
    label.setBounds(0,0, 50, 50);
    label.setBackground(Color.red);
    label.setOpaque(true);

    this.add(label);
    this.setVisible(true);
  }

  // for a typable character that is sent from keyboard
  @Override
  public void keyTyped(KeyEvent e) {
    // switch(Character.toLowerCase(e.getKeyChar())) {
    //   case 'w': label.setLocation(label.getX(), label.getY()-10);
    //     break;
    //   case 'a': label.setLocation(label.getX()-10, label.getY());
    //     break;
    //   case 's': label.setLocation(label.getX(), label.getY()+10);
    //     break;
    //   case 'd': label.setLocation(label.getX()+10, label.getY());
    //     break;
    // }
  }

  // for any unicode character that is received as input
  @Override
  public void keyPressed(KeyEvent e) {
    switch(e.getKeyCode()) {
      case 87: label.setLocation(label.getX(), label.getY()-this.speed);
        break;
      case 65: label.setLocation(label.getX()-this.speed, label.getY());
        break;
      case 83: label.setLocation(label.getX(), label.getY()+this.speed);
        break;
      case 68: label.setLocation(label.getX()+this.speed, label.getY());
        break;
    }
  }

  @Override
  public void keyReleased(KeyEvent e) {
    // System.out.println("You released key char: " + e.getKeyChar());
    // System.out.println("You released key code: " + e.getKeyCode());
  }
}