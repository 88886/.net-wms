using System;
using System.DirectoryServices;
using System.Collections;

namespace Tools
{
	/// <summary>
	/// IISWebServer��״̬
	/// </summary>
	public enum IISServerState
	{
		/// <summary>
		/// 
		/// </summary>
		Starting = 1,
		/// <summary>
		/// 
		/// </summary>
		Started = 2,
		/// <summary>
		/// 
		/// </summary>
		Stopping = 3,
		/// <summary>
		/// 
		/// </summary>
		Stopped = 4,
		/// <summary>
		/// 
		/// </summary>
		Pausing = 5,
		/// <summary>
		/// 
		/// </summary>
		Paused = 6,
		/// <summary>
		/// 
		/// </summary>
		Continuing = 7

	}
	/// <summary>
	/// IISManagement ��ժҪ˵����
	/// </summary>
	public class IISManagement
	{
		/// <summary>
		/// 
		/// </summary>
		public IISWebServerCollection WebServers = new IISWebServerCollection();
		internal static string Machinename = "localhost";
		/// <summary>
		/// 
		/// </summary>
		public IISManagement()
		{
			start();
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="MachineName">������,Ĭ��ֵΪlocalhost</param>
		public IISManagement(string MachineName)
		{
			if( MachineName.ToString() != "" )
				Machinename = MachineName;
			start();
		}

		private void start()
		{
			DirectoryEntry Service = new DirectoryEntry("IIS://" + Machinename + "/W3SVC");
			DirectoryEntry Server;
			DirectoryEntry Root = null;
			DirectoryEntry VirDir;

			IEnumerator ie = Service.Children.GetEnumerator();
			IEnumerator ieRoot;
			IISWebServer item;
			IISWebVirtualDir item_virdir;
			bool finded = false;
			while(ie.MoveNext())
			{
				Server = (DirectoryEntry)ie.Current;
				if( Server.SchemaClassName == "IIsWebServer" )	
				{
					item = new IISWebServer();
					item.index = Convert.ToInt16(Server.Name);
					item.ServerComment = (string)Server.Properties["ServerComment"][0];
					item.AccessRead = (bool)Server.Properties["AccessRead"][0];
					item.AccessScript = (bool)Server.Properties["AccessScript"][0];
					item.DefaultDoc = (string)Server.Properties["DefaultDoc"][0];
					item.EnableDefaultDoc = (bool)Server.Properties["EnableDefaultDoc"][0];
					item.EnableDirBrowsing = (bool)Server.Properties["EnableDirBrowsing"][0];
					ieRoot = Server.Children.GetEnumerator();

					while( ieRoot.MoveNext() )
					{
						Root = (DirectoryEntry)ieRoot.Current;
						if( Root.SchemaClassName == "IIsWebVirtualDir" )
						{
							finded = true;
							break;
						}
					}
					if( finded )
					{
						item.Path = Root.Properties["path"][0].ToString();
					}

                    
                    if (string.IsNullOrEmpty(Convert.ToString(Server.Properties["Serverbindings"].Value)))
                    {
                        continue;
                    }
                    item.Port = Convert.ToInt16(Server.Properties["Serverbindings"][0].ToString().Split(':')[1]);
					this.WebServers.Add_(item);
					ieRoot = Root.Children.GetEnumerator();
					while( ieRoot.MoveNext() )
					{
						VirDir = (DirectoryEntry)ieRoot.Current;
						if( VirDir.SchemaClassName != "IIsWebVirtualDir" && VirDir.SchemaClassName != "IIsWebDirectory" )
							continue;
						item_virdir = new IISWebVirtualDir(item.ServerComment);
						item_virdir.Name = VirDir.Name;
						item_virdir.AccessRead = (bool)VirDir.Properties["AccessRead"][0];
						item_virdir.AccessScript = (bool)VirDir.Properties["AccessScript"][0];
						item_virdir.DefaultDoc = (string)VirDir.Properties["DefaultDoc"][0];
						item_virdir.EnableDefaultDoc = (bool)VirDir.Properties["EnableDefaultDoc"][0];
						if( VirDir.SchemaClassName == "IIsWebVirtualDir" )
						{
							item_virdir.Path = (string)VirDir.Properties["Path"][0];
						}
						else if( VirDir.SchemaClassName == "IIsWebDirectory" )
						{
							item_virdir.Path = Root.Properties["Path"][0] + "\\" + VirDir.Name;
						}
						item.WebVirtualDirs.Add_(item_virdir);
					}
				}
			}
		}

		/// <summary>
		/// ����վ��
		/// </summary>
		/// <param name="iisServer"></param>
		public static void CreateIISWebServer(IISWebServer iisServer)
		{
			if (true ==  ExistsIISWebServer(iisServer.Port))
			{
				throw new Exception("ָ���Ķ˿ںŲ����ã�ԭ�����������վ���Ӧ�ó���ռ�ã�");

			}

			if( iisServer.ServerComment.ToString() == "" )
				throw(new Exception("IISWebServer��ServerComment����Ϊ��!"));
			DirectoryEntry Service = new DirectoryEntry("IIS://" + IISManagement.Machinename + "/W3SVC");
			DirectoryEntry Server;
			int i = 0 ;
			IEnumerator ie = Service.Children.GetEnumerator();

			while(ie.MoveNext())
			{
				Server = (DirectoryEntry)ie.Current;
				if( Server.SchemaClassName == "IIsWebServer" )	
				{
					if( Convert.ToInt16(Server.Name) > i )
						i = Convert.ToInt16(Server.Name);
					
					Server.Invoke("start",new object[0]);
					
				}
			}

			i++;

			try
			{
				iisServer.index = i;
				Server = Service.Children.Add(i.ToString() , "IIsWebServer");
				Server.Properties["ServerComment"][0] = iisServer.ServerComment;
				Server.Properties["Serverbindings"].Add(":" + iisServer.Port + ":");
				Server.Properties["AccessScript"][0] = iisServer.AccessScript;
				Server.Properties["AccessRead"][0] = iisServer.AccessRead;
				Server.Properties["EnableDirBrowsing"][0] = iisServer.EnableDirBrowsing;
				Server.Properties["DefaultDoc"][0] = iisServer.DefaultDoc;
				Server.Properties["EnableDefaultDoc"][0] = iisServer.EnableDefaultDoc;

				
				DirectoryEntry	root = Server.Children.Add("Root","IIsWebVirtualDir");
				root.Properties["path"][0] = iisServer.Path;


				root.Invoke("AppCreate",false);


				//CreateAppPool(iisServer.ServerComment);
				//AssignAppPool(root,iisServer.ServerComment);

				Service.CommitChanges();
				Server.CommitChanges();
				root.CommitChanges();

				Server.Invoke("start",new object[0]);
			}
			catch(Exception es)
			{
				throw(es);
			}
		}

		private DirectoryEntry CreateVDir (string vdirname)
		{
			DirectoryEntry newvdir;
			DirectoryEntry root=new DirectoryEntry("IIS://localhost/W3SVC/1/Root");
			newvdir=root.Children.Add(vdirname, "IIsWebVirtualDir");
			newvdir.Properties["Path"][0]= "c:\\inetpub\\wwwroot";
			newvdir.Properties["AccessScript"][0] = true;
			newvdir.CommitChanges();
			return newvdir;
		}

		public static void CreateAppPool(string AppPoolName)
		{
			DirectoryEntry newpool;
			DirectoryEntry apppools=new DirectoryEntry("IIS://localhost/W3SVC/AppPools");
			
			newpool=apppools.Children.Add(AppPoolName, "IIsApplicationPool");
			newpool.CommitChanges();
		
		}

		public static void AssignAppPool(DirectoryEntry newvdir, string AppPoolName)
		{
			object[] param={0, AppPoolName, true};
			newvdir.Invoke("AppCreate3", param);
		}
	
		/// <summary>
		/// ɾ��IISWebServer
		/// </summary>
		public static void RemoveIISWebServer(string ServerComment)
		{
			DirectoryEntry Service = new DirectoryEntry("IIS://" + IISManagement.Machinename + "/W3SVC");
			DirectoryEntry Server;
			IEnumerator ie = Service.Children.GetEnumerator();

			ServerComment = ServerComment.ToLower();
			while(ie.MoveNext())
			{
				Server = (DirectoryEntry)ie.Current;
				if( Server.SchemaClassName == "IIsWebServer" )	
				{
					if( Server.Properties["ServerComment"][0].ToString().ToLower() == ServerComment )
					{
						Service.Children.Remove(Server);
						Service.CommitChanges();
						return ;
					}
				}
			}
		}

		/// <summary>
		/// ɾ��IISWebServer
		/// </summary>
		public static void RemoveIISWebServer(int index)
		{
			DirectoryEntry Service = new DirectoryEntry("IIS://" + IISManagement.Machinename + "/W3SVC");
			try
			{
				DirectoryEntry Server = new DirectoryEntry("IIS://" + IISManagement.Machinename + "/W3SVC/" + index);
				if( Server != null )
				{
					Service.Children.Remove(Server);
					Service.CommitChanges();
				}
				else
				{
					throw(new Exception("�Ҳ�����IISWebServer"));
				}
			}
			catch
			{
				throw(new Exception("�Ҳ�����IISWebServer"));
			}
		}

		/// <summary>
		/// ����Ƿ����IISWebServer
		/// </summary>
		/// <param name="ServerComment">��վ˵��</param>
		/// <returns></returns>
		public static bool ExistsIISWebServer(string ServerComment)
		{
			ServerComment = ServerComment.Trim();
			DirectoryEntry Service = new DirectoryEntry("IIS://" + IISManagement.Machinename + "/W3SVC");
			DirectoryEntry Server = null;
			IEnumerator ie = Service.Children.GetEnumerator();

			string comment;
			while(ie.MoveNext())
			{
				Server = (DirectoryEntry)ie.Current;
				if( Server.SchemaClassName == "IIsWebServer" )	
				{
					comment = Server.Properties["ServerComment"][0].ToString().ToLower().Trim();
					if( comment == ServerComment.ToLower() )	
					{
						return true;
					}
				}
			}

			return false;
		}

		/// <summary>
		/// ����Ƿ����IISWebServer
		/// </summary>
		/// <param name="ServerComment">�˿ں�</param>
		/// <returns></returns>
		public static bool ExistsIISWebServer(int PortNumber)
		{
			DirectoryEntry Service = new DirectoryEntry("IIS://" + IISManagement.Machinename + "/W3SVC");
			DirectoryEntry Server = null;
			IEnumerator ie = Service.Children.GetEnumerator();

			int port;
			while(ie.MoveNext())
			{
				Server = (DirectoryEntry)ie.Current;
				if( Server.SchemaClassName == "IIsWebServer" )	
				{
					string strPort = Server.Properties["Serverbindings"][0].ToString().ToLower();
					port = int.Parse(strPort.Substring(1,(strPort.Length - 2)).Trim());
					if( port == PortNumber )	
					{
						return true;
					}
				}
			}

			return false;
		}

		/// <summary>
		/// ����ָ����IISWebServer
		/// </summary>
		/// <param name="ServerComment"></param>
		/// <returns></returns>
		internal static DirectoryEntry returnIISWebserver(string ServerComment)
		{
			ServerComment = ServerComment.Trim();
			DirectoryEntry Service = new DirectoryEntry("IIS://" + IISManagement.Machinename + "/W3SVC");
			DirectoryEntry Server = null;
			IEnumerator ie = Service.Children.GetEnumerator();

			string comment;
			while(ie.MoveNext())
			{
				Server = (DirectoryEntry)ie.Current;
				if( Server.SchemaClassName == "IIsWebServer" )	
				{
					comment = Server.Properties["ServerComment"][0].ToString().ToLower().Trim();
					if( comment == ServerComment.ToLower() )	
					{
						return Server;
					}
				}
			}

			return null;
		}


		/// <summary>
		///  ����ָ����IISWebServer
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		internal static DirectoryEntry returnIISWebserver(int index)
		{
			DirectoryEntry Server = new DirectoryEntry("IIS://" + IISManagement.Machinename + "/W3SVC/" + index);
			try
			{
				IEnumerator ie = Server.Children.GetEnumerator();
				return Server;
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// �޸��������IISWebServer������ͬ��վ˵����վ������
		/// </summary>
		/// <param name="iisServer">������IISWebServer</param>
		public static void EditIISWebServer(IISWebServer iisServer)
		{
			if( iisServer.index == -1 )
				throw(new Exception("�Ҳ���������վ��!"));
			DirectoryEntry Service = new DirectoryEntry("IIS://" + IISManagement.Machinename + "/W3SVC");
			DirectoryEntry Server;

			IEnumerator ie = Service.Children.GetEnumerator();

			while(ie.MoveNext())
			{
				Server = (DirectoryEntry)ie.Current;
				if( Server.SchemaClassName == "IIsWebServer" )	
				{
					if( Server.Properties["Serverbindings"][0].ToString() == ":" + iisServer.Port + ":" )	
					{
						Server.Invoke("stop",new object[0]);
					}
				}
			}

			Server = returnIISWebserver(iisServer.index);
			if( Server == null )
			{
				throw( new Exception("�Ҳ���������վ��!"));
			}

			try
			{

				Server.Properties["ServerComment"][0] = iisServer.ServerComment;
				Server.Properties["Serverbindings"][0] = ":" + iisServer.Port + ":";
				Server.Properties["AccessScript"][0] = iisServer.AccessScript;
				Server.Properties["AccessRead"][0] = iisServer.AccessRead;
				Server.Properties["EnableDirBrowsing"][0] = iisServer.EnableDirBrowsing;
				Server.Properties["DefaultDoc"][0] = iisServer.DefaultDoc;
				Server.Properties["EnableDefaultDoc"][0] = iisServer.EnableDefaultDoc;

				ie = Server.Children.GetEnumerator();
				ie.MoveNext();
				DirectoryEntry	root = (DirectoryEntry)ie.Current;
				root.Properties["path"][0] = iisServer.Path;
				

				Server.CommitChanges();
				root.CommitChanges();
				
				Server.Invoke("start",new object[0]);
			}
			catch(Exception es)
			{
				throw(es);
			}
		}

		/// <summary>
		/// ��������վ�����վ˵��
		/// </summary>
		/// <returns></returns>
		public static ArrayList returnIISServerComment()
		{
			DirectoryEntry Service = new DirectoryEntry("IIS://" + IISManagement.Machinename + "/W3SVC");
			DirectoryEntry Server;

			ArrayList list = new ArrayList();
			IEnumerator ie = Service.Children.GetEnumerator();

			while(ie.MoveNext())
			{
				Server = (DirectoryEntry)ie.Current;
				if( Server.SchemaClassName == "IIsWebServer" )	
				{
					list.Add(Server.Properties["ServerComment"][0]);
				}
			}

			return list;
		}

		/// <summary>
		/// ��������Ŀ¼
		/// </summary>
		/// <param name="iisVir"></param>
		/// <param name="deleteIfExist"></param>
		public static void CreateIISWebVirtualDir(IISWebVirtualDir iisVir , bool deleteIfExist)
		{
			if( iisVir.Parent == null )
				throw(new Exception("IISWebVirtualDirû��������IISWebServer!"));

			DirectoryEntry Service = new DirectoryEntry("IIS://" + IISManagement.Machinename + "/W3SVC");
			DirectoryEntry Server = returnIISWebserver(iisVir.Parent.index);
			
			if( Server == null )
			{
				throw( new Exception("�Ҳ���������վ��!"));
			}
			
			IEnumerator ie = Server.Children.GetEnumerator();
			ie.MoveNext();

			Server = (DirectoryEntry)ie.Current;
			if( deleteIfExist )
			{
				DirectoryEntry VirDir;
				ie = Server.Children.GetEnumerator();
				while(ie.MoveNext())
				{
					VirDir = (DirectoryEntry)ie.Current;
					if( VirDir.Name.ToLower().Trim() == iisVir.Name.ToLower() )
					{
						Server.Children.Remove(VirDir);
						Server.CommitChanges();
						break;
					}
				}
			}

			try
			{
				DirectoryEntry vir;
				vir = Server.Children.Add(iisVir.Name , "IIsWebVirtualDir");
				vir.Properties["Path"][0] = iisVir.Path;
				vir.Properties["DefaultDoc"][0] = iisVir.DefaultDoc;
				vir.Properties["EnableDefaultDoc"][0] = iisVir.EnableDefaultDoc;
				vir.Properties["AccessScript"][0] = iisVir.AccessScript;
				vir.Properties["AccessRead"][0] = iisVir.AccessRead;

				vir.Invoke("AppCreate",true);

				Server.CommitChanges();
				vir.CommitChanges();

			}
			catch(Exception es)
			{
				throw(es);
			}

		}

		/// <summary>
		/// ɾ������Ŀ¼
		/// </summary>
		/// <param name="iisVir"></param>
		public static void RemoveIISWebVirtualDir(IISWebVirtualDir iisVir)
		{
			DirectoryEntry Service = new DirectoryEntry("IIS://" + IISManagement.Machinename + "/W3SVC");
			DirectoryEntry Server = returnIISWebserver(iisVir.Parent.index);
			
			if( Server == null )
			{
				throw(new Exception( "�Ҳ���������վ��!"));
			}
			
			IEnumerator ie = Server.Children.GetEnumerator();
			ie.MoveNext();

			Server = (DirectoryEntry)ie.Current;

				DirectoryEntry VirDir;
				ie = Server.Children.GetEnumerator();
			while(ie.MoveNext())
			{
				VirDir = (DirectoryEntry)ie.Current;
				if( VirDir.Name.ToLower() == iisVir.Name.ToLower() )
				{
					Server.Children.Remove(VirDir);
					Server.CommitChanges();
					return;
				}
			}
			throw(new Exception( "�Ҳ�������������Ŀ¼!"));
		}

	}

	/// <summary>
	/// 
	/// </summary>
	public class IISWebServerCollection:CollectionBase
	{
		
		/// <summary>
		/// 
		/// </summary>
		public IISWebServer this[int Index]
		{
			get
			{
				return (IISWebServer)this.List[Index];
				
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public IISWebServer this[string ServerComment]
		{
			get
			{
				ServerComment = ServerComment.ToLower().Trim();
				IISWebServer list;
				for(int i = 0 ; i < this.List.Count ; i++)
				{
					list = (IISWebServer)this.List[i];
					if(list.ServerComment.ToLower().Trim() == ServerComment)
						return list;
				}
				return null;
			}
		}

		internal void Add_(IISWebServer WebServer)
		{
			this.List.Add(WebServer);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="WebServer"></param>
		public void Add(IISWebServer WebServer)
		{
			try
			{
				this.List.Add(WebServer);
				IISManagement.CreateIISWebServer(WebServer);
			}
			catch
			{
				throw(new Exception("����������󣬿�����ĳ�ڵ㽫�ýڵ���ϼ��ڵ���Ϊ���Լ����Ӽ�����"));
			}
			
		}

		/// <summary>
		/// �Ƿ����ָ������վ
		/// </summary>
		/// <param name="ServerComment"></param>
		/// <returns></returns>
		public bool Contains(string ServerComment)
		{
			ServerComment = ServerComment.ToLower().Trim();
			for( int i = 0 ; i < this.List.Count ; i++ )
			{
				IISWebServer server = this[i];
				if( server.ServerComment.ToLower().Trim() == ServerComment )
					return true;
			}
			return false;
		}

		/// <summary>
		/// �Ƿ����ָ������վ
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public bool Contains(int index)
		{
			for( int i = 0 ; i < this.List.Count ; i++ )
			{
				IISWebServer server = this[i];
				if( server.index == index )
					return true;
			}
			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="WebServers"></param>
		public void AddRange(IISWebServer [] WebServers)
		{
			for(int i = 0 ; i <= WebServers.GetUpperBound(0) ; i++)
			{
				Add(WebServers[i]);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="WebServer"></param>
		public void Remove(IISWebServer WebServer)
		{
			for(int i=0;i<this.List.Count;i++)
			{
				if((IISWebServer)this.List[i] == WebServer)
				{
					this.List.RemoveAt(i);
					return;
				}
			}
			IISManagement.RemoveIISWebServer(WebServer.index);
		}
	}
	

	//////////////////
	/// <summary>
	/// 
	/// </summary>
	public class IISWebServer
	{
		/// <summary>
		/// 
		/// </summary>
		internal int index = -1;
		/// <summary>
		/// 
		/// </summary>
		public IISWebVirtualDirCollection WebVirtualDirs ;
		/// <summary>
		/// ��վ˵��
		/// </summary>
		public string ServerComment = "Way";
		/// <summary>
		/// �ű�֧��
		/// </summary>
		public bool AccessScript = true;
		/// <summary>
		/// ��ȡ
		/// </summary>
		public bool AccessRead = true;
		/// <summary>
		/// д��
		/// </summary>
		public bool AccessWrite = true;

		/// <summary>
		/// ����·��
		/// </summary>
		public string Path = "c:\\";
		/// <summary>
		/// �˿�
		/// </summary>
		public int Port = 80;
		/// <summary>
		/// Ŀ¼���
		/// </summary>
		public bool EnableDirBrowsing = false;
		/// <summary>
		/// Ĭ���ĵ�
		/// </summary>
		public string DefaultDoc = "index.aspx";
		/// <summary>
		/// ʹ��Ĭ���ĵ�
		/// </summary>
		public bool EnableDefaultDoc =true;

		/// <summary>
		/// IISWebServer��״̬
		/// </summary>
		public IISServerState ServerState
		{
			get
			{
				DirectoryEntry server = IISManagement.returnIISWebserver(this.index);
				if( server == null )
					throw(new Exception("�Ҳ�����IISWebServer"));
				switch (server.Properties["ServerState"][0].ToString())
				{
					case "2":
						return IISServerState.Started;
					case "4":
						return IISServerState.Stopped;
					case "6":
						return IISServerState.Paused;
				}
				return IISServerState.Stopped;
			}
		}

		/// <summary>
		/// ֹͣIISWebServer
		/// </summary>
		public void Stop()
		{
			DirectoryEntry Server;
			if( index == -1 )
				throw(new Exception("��IIS�Ҳ�����IISWebServer!"));
			try
			{
				Server = new DirectoryEntry("IIS://" + IISManagement.Machinename + "/W3SVC/" + index);
				if( Server != null )
					Server.Invoke("stop" , new object[0]);
				else
					throw(new Exception("��IIS�Ҳ�����IISWebServer!"));
			}
			catch
			{
				throw(new Exception("��IIS�Ҳ�����IISWebServer!"));
			}
		}

		/// <summary>
		/// �ѻ�����Ϣ�ĸ��ĸ��µ�IIS
		/// </summary>
		public void CommitChanges()
		{
			IISManagement.EditIISWebServer(this);
		}

		/// <summary>
		/// ����IISWebServer
		/// </summary>
		public void Start()
		{
			if( index == -1 )
				throw(new Exception("��IIS�Ҳ�����IISWebServer!"));
			
			DirectoryEntry Service = new DirectoryEntry("IIS://" + IISManagement.Machinename + "/W3SVC");
			DirectoryEntry Server;
			IEnumerator ie = Service.Children.GetEnumerator();

			while(ie.MoveNext())
			{
				Server = (DirectoryEntry)ie.Current;
				if( Server.SchemaClassName == "IIsWebServer" )	
				{
					if( Server.Properties["Serverbindings"][0].ToString() == ":" + this.Port + ":" )	
					{
						Server.Invoke("stop",new object[0]);
					}
				}
			}

			try
			{
				Server = new DirectoryEntry("IIS://" + IISManagement.Machinename + "/W3SVC/" + index);
				if( Server != null )
				{
					Server.Invoke("start" , new object[0]);
				}
				else
				{
					throw(new Exception("��IIS�Ҳ�����IISWebServer!"));
				}
			}
			catch
			{
				throw(new Exception("��IIS�Ҳ�����IISWebServer!"));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public IISWebServer()
		{
			WebVirtualDirs = new IISWebVirtualDirCollection(this);
		}
///////////////////////////////////////////
	}

	/// <summary>
	/// 
	/// </summary>
	public class IISWebVirtualDirCollection:CollectionBase
	{
		/// <summary>
		/// 
		/// </summary>
		public IISWebServer Parent = null;

		/// <summary>
		/// 
		/// </summary>
		public IISWebVirtualDir this[int Index]
		{
			get
			{
				return (IISWebVirtualDir)this.List[Index];
				
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public IISWebVirtualDir this[string Name]
		{
			get
			{
				Name = Name.ToLower();
				IISWebVirtualDir list;
				for(int i = 0 ; i < this.List.Count ; i++)
				{
					list = (IISWebVirtualDir)this.List[i];
					if(list.Name.ToLower() == Name)
						return list;
				}
				return null;
			}
		}


		internal void Add_(IISWebVirtualDir WebVirtualDir)
		{
			try
			{
				this.List.Add(WebVirtualDir);
			}
			catch
			{
				throw(new Exception("����������󣬿�����ĳ�ڵ㽫�ýڵ���ϼ��ڵ���Ϊ���Լ����Ӽ�����"));
			}
			
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="WebVirtualDir"></param>
		public void Add(IISWebVirtualDir WebVirtualDir)
		{
			WebVirtualDir.Parent = this.Parent;
			try
			{
				this.List.Add(WebVirtualDir);
				
			}
			catch
			{
				throw(new Exception("����������󣬿�����ĳ�ڵ㽫�ýڵ���ϼ��ڵ���Ϊ���Լ����Ӽ�����"));
			}
			IISManagement.CreateIISWebVirtualDir(WebVirtualDir , true);
			
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="WebVirtualDirs"></param>
		public void AddRange(IISWebVirtualDir [] WebVirtualDirs)
		{
			for(int i = 0 ; i <= WebVirtualDirs.GetUpperBound(0) ; i++)
			{
				Add(WebVirtualDirs[i]);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="WebVirtualDir"></param>
		public void Remove(IISWebVirtualDir WebVirtualDir)
		{
			for(int i=0;i<this.List.Count;i++)
			{
				if((IISWebVirtualDir)this.List[i] == WebVirtualDir)
				{
					this.List.RemoveAt(i);
					IISManagement.RemoveIISWebVirtualDir(WebVirtualDir);
					return;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Parent"></param>
		public IISWebVirtualDirCollection(IISWebServer Parent)
		{
			this.Parent = Parent;
		}
	}


	///////////////
	/// <summary>
	/// 
	/// </summary>
	public class IISWebVirtualDir
	{
		/// <summary>
		/// 
		/// </summary>
		public IISWebServer Parent = null;
		/// <summary>
		/// ����Ŀ¼����
		/// </summary>
		public string Name = "Way";
		/// <summary>
		/// ��ȡ
		/// </summary>
		public bool AccessRead = true;
		/// <summary>
		/// д��
		/// </summary>
		public bool AccessWrite = true;
		/// <summary>
		/// �ű�֧��
		/// </summary>
		public bool AccessScript = true;
		/// <summary>
		/// ����·��
		/// </summary>
		public string Path = "c:\\";
		/// <summary>
		/// Ĭ���ĵ�
		/// </summary>
		public string DefaultDoc = "index.aspx";
		/// <summary>
		/// ʹ��Ĭ���ĵ�
		/// </summary>
		public bool EnableDefaultDoc =true;
		/// <summary>
		/// ��������վ����վ˵��
		/// </summary>
		public string WebServer = "";

		/// <summary>
		/// 
		/// </summary>
		/// <param name="WebServerName"></param>
		public IISWebVirtualDir(string WebServerName)
		{
			if( WebServerName.ToString() == "" )
				throw(new Exception("WebServerName����Ϊ��!"));
			this.WebServer = WebServerName;
		}
		/// <summary>
		/// 
		/// </summary>
		public IISWebVirtualDir()
		{

		}
	}
}
