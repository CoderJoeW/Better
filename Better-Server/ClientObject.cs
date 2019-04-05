using System;
using System.Net.Sockets;

namespace Better_Server {
    public class ClientObject {
        public TcpClient socket;
        public NetworkStream myStream;
        public int connectionID;
        public byte[] recieveBuffer;
        public ByteBuffer buffer;

        public ClientObject(TcpClient _socket,int _connectionID) {
            if(_socket == null) {
                return;
            }

            socket = _socket;
            connectionID = _connectionID;

            socket.NoDelay = true;

            socket.ReceiveBufferSize = 4096;
            socket.SendBufferSize = 4096;

            myStream = socket.GetStream();

            recieveBuffer = new byte[4096];

            myStream.BeginRead(recieveBuffer, 0, socket.ReceiveBufferSize, ReceiveCallback, null);

            Console.WriteLine("Incoming connection from {0}", socket.Client.RemoteEndPoint.ToString());
        }

        private void ReceiveCallback(IAsyncResult result) {
            try {
                int readBytes = myStream.EndRead(result);

                if(readBytes <= 0) {
                    CloseConnection();
                    return;
                }

                byte[] newBytes = new byte[readBytes];
                Buffer.BlockCopy(recieveBuffer, 0, newBytes,0,readBytes);

                ServerHandleData.HandleData(connectionID, newBytes);
                myStream.BeginRead(recieveBuffer, 0, socket.ReceiveBufferSize, ReceiveCallback, null);
            } catch (Exception) {
                CloseConnection();
                throw;
            }
        }

        private void CloseConnection() {
            Console.WriteLine("Connection from {0} has been terminated.", socket.Client.RemoteEndPoint.ToString());
            socket.Close();
            socket = null;
        }
    }
}
