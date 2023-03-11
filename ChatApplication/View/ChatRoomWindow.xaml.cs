using ChatApplication.DB;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ChatApplication.View
{
    /// <summary>
    /// Логика взаимодействия для ChatRoomWindow.xaml
    /// </summary>
    public partial class ChatRoomWindow : Window
    {
        public ChatRoomWindow()
        {
            InitializeComponent();
            HelloLbl.Content += App.user.Username;
            Update();
        }

        private void Update()
        {
            var resLst = new List<ChatRoomsSupportClass>();

            var a = App.db.Employee_ChatRoom.Where(x => x.Id_Employee == App.user.Id).ToList();
            foreach (var item in a)
            {
                var lastMessages = App.db.ChatMessage.Where(x => x.Chatroom_Id == item.Id_ChatRoom);
                ChatMessage lastMessage = null;
                foreach (var lastmess in lastMessages)
                {
                    lastMessage = lastmess;
                }
                resLst.Add(new ChatRoomsSupportClass(item.Id, item.ChatRoom.Topic, lastMessage?.Date.ToString()));
            }

            ChatRoomsLstView.ItemsSource = resLst;
        }

        private void ChatRoomsLstView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var idEmployeeChatRoom = (ChatRoomsLstView.SelectedItem as ChatRoomsSupportClass).IdEmployeeChatRoom;
            var item = App.db.Employee_ChatRoom.Where(x=>x.Id == idEmployeeChatRoom).FirstOrDefault();
            var window = new ChatWindow(item);
            window.Closed += (q,w) => Update();
            window.Show();  
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void EmployeeFinderBtn_Click(object sender, RoutedEventArgs e)
        {
            new EmployeeFinderWindow().Show();
        }
    }

    public class ChatRoomsSupportClass
    {
        public int IdEmployeeChatRoom { get; set; }
        public string Topic { get; set; }
        public string LastMessageDate { get; set; }

        public ChatRoomsSupportClass(int id, string topic, string lastMessageDate)
        {
            IdEmployeeChatRoom = id;
            Topic = topic;
            LastMessageDate = lastMessageDate;
        }
    }
}
