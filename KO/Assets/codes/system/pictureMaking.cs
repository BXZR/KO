using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class pictureMaking : MonoBehaviour {

	//有关截图处理的脚本，但说实话有点卡啊

	//尝试存储游戏截图到指定的文件夹，例如StreaAssest文件夹
	//用于全部的图片保存效果，这个类是图像保存总控制器
	//保存的底层方法使用
	private List<string> theNameStringForPictures;
	private List<Sprite> Pictures;

	private  string theDictionaryPath ;//文件夹的路径
	Texture2D theTexture;//加载截图
	string path;//保存截图
	private int theNameIndex;//图片的下标名字

 
	void Start () 
	{
		theDictionaryPath =  Application.dataPath + @"/StreamingAssets/Pictures/";//文件夹的路径
	}
	//全局静态调用保存图片方法
	//用这种方法尽可能减少程序的改动，必经这只是一个小的附加功能而已
	public   void savePicture()
	{
		//预防空引用
		if (theNameStringForPictures == null)
		theNameStringForPictures = new List<string> ();
		//这些才是保存图像最重要的功能
		theNameIndex  = theNameStringForPictures.Count;
        theNameStringForPictures.Add (theNameIndex .ToString());//用于计数
		StartCoroutine (OnScreenCapture ());//等待一帧（也就是等待渲染完成）
	}
	//删除所有的图片也就是刷新
	public void deleteAllPictures()
	{
		 
		//预防空引用
		if (theNameStringForPictures == null)
			theNameStringForPictures = new List<string> ();
		//删除所有图片 
		if (Directory.Exists(theDictionaryPath))
		{  
			DirectoryInfo direction = new DirectoryInfo(theDictionaryPath);  
			FileInfo[] files = direction.GetFiles("*",SearchOption.AllDirectories);  
			for(int i=0;i<files.Length;i++)
			{  
				File.Delete (files [i].FullName); 
			}  
		}  
	    //除了图片之外的系统删除
		theNameStringForPictures.Clear();
		theNameIndex = 0;
	}
	//删除所有的图片也就是刷新
	public void loadAllPictures()
	{
		//删除所有图片 
		if (Directory.Exists(theDictionaryPath))
		{  
			DirectoryInfo direction = new DirectoryInfo(theDictionaryPath);  
			FileInfo[] files = direction.GetFiles("*",SearchOption.AllDirectories);  
			for(int i=0;i<files.Length;i++)
			{  
				File.Delete (files [i].FullName); 
			}  
		}  
		//除了图片之外的系统删除
		theNameStringForPictures.Clear();
		theNameIndex = 0;
	}


	//从文件夹中获取所有的Texture,并把这些Texture做成Sprite最终返回给UI制作但愿做图片
	public  List<Sprite> loadTextures()
	{
		//预防空引用
		if (Pictures == null)
			Pictures = new List<Sprite> ();
		Pictures.Clear ();//保证在获取值之前是空的

		//获取保存到的图片并加以处理
		Texture2D useTexture ;//中间变量保存
		if (theNameStringForPictures != null)
		{
			for (int i = 0; i < theNameStringForPictures.Count; i++) 
			{
				useTexture = loadPicture (theNameStringForPictures [i]);
				Pictures.Add (makeSprite (useTexture));
			}
		} 
		else 
		{

			if (Directory.Exists(theDictionaryPath))
			{ 
				int pictureCount = 0;
				DirectoryInfo direction = new DirectoryInfo(theDictionaryPath);  
				FileInfo[] files = direction.GetFiles("*",SearchOption.AllDirectories);  
				for(int i=0;i<files.Length   ;i++)
				{  
					if (files [i].Name.EndsWith (systemValues .thePicEndType))
					{
						useTexture = loadPicture (files [i].FullName, false);
						Pictures.Add (makeSprite (useTexture));
						pictureCount++;
						if (pictureCount >= systemValues.pictureLoadCount || pictureCount >= files.Length)
							break;
					}

				}  
			} 
		}
		return Pictures;
	}
		

	//确定保存的图像名称(有一定的可操纵性)
	private string theFileNameMake(string theFileName)
	{
		return  Application.dataPath + @"/StreamingAssets/Pictures/" + theFileName+systemValues .thePicEndType;
	}


	IEnumerator OnScreenCapture ()
	{
		 yield return new WaitForEndOfFrame();
		try
		{
			int width = Screen.width;
			int height = Screen.height;
			 
			int widthUse = (int)(width*0.8f);
			int heightUse =(int)(height*0.8f);


			Texture2D tex = new Texture2D (widthUse, heightUse, TextureFormat.RGB24, false);//新建一张图
			tex.ReadPixels (new Rect (width*0.1f, height*0.1f, widthUse, heightUse), 0, 0, true);//从屏幕开始读点

			 

			byte[] imagebytes = tex.EncodeToJPG ();//用的是JPG(这种比较小)
			//使用它压缩实时产生的纹理，压缩过的纹理使用更少的显存并可以更快的被渲染
			//通过true为highQuality参数将抖动压缩期间源纹理，这有助于减少压缩伪像
			//因为压缩后的图像不作为纹理使用，只是一张用于展示的图
			//但稍微慢一些这个小功能暂时貌似还用不到
			tex.Compress (false);
			tex.Apply();
			Texture2D mScreenShotImgae = tex;

			if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
			{
				path = Application.persistentDataPath + theNameIndex + systemValues .thePicEndType;
				string origin = path;

				string destination = "/mnt/sdcard/moliputao";

				if(!Directory.Exists(destination))
				{
					Directory.CreateDirectory(destination);
				}

				 destination = destination + "/" +   theNameIndex + systemValues .thePicEndType;

				if(System.IO.File.Exists(origin))
				{
					System.IO.File.Move(origin,destination);
				}
				path = destination;

			}
			else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
			{ 
				//path = Application.dataPath;
				//path = path.Replace ("/Assets", "/"+  theNameIndex + ".png");
				path = theFileNameMake( theNameIndex.ToString());
			}

			File.WriteAllBytes (path, imagebytes);
			mScreenShotImgae = tex;

		} catch (System.Exception e)
		{
			Debug.Log ("ScreenCaptrueError:" + e);
		}
	}
	 
	//从文件中读取图片保存为Texture然后返回这个Texture
	private  Texture2D loadPicture(string theFileName="suck",bool change = true)
	{
		try
		{
		//创建文件读取流
			FileStream fileStream ;
			if(change)
			fileStream = new FileStream( theFileNameMake(theFileName), FileMode.Open, FileAccess.Read);
			else
			fileStream = new FileStream( theFileName, FileMode.Open, FileAccess.Read);	
			
			fileStream.Seek(0, SeekOrigin.Begin);
			//创建文件长度缓冲区
			byte[] bytes = new byte[fileStream.Length]; 
			//读取文件
			fileStream.Read(bytes, 0, (int)fileStream.Length);
			//释放文件读取流
			fileStream.Close();
			fileStream.Dispose();
			fileStream = null;
			//创建Texture
			int width=800;
			int height=640;
			theTexture = new Texture2D(width, height);
			theTexture.LoadImage(bytes);
		}
		catch
		{
			theTexture = null;
		}
		return theTexture;

	}
	//将texture转sprite超级适配器
	private  Sprite makeSprite(Texture2D theTextureIn)
	{
		return Sprite .Create(theTextureIn,new Rect (0,0,theTextureIn.width,theTextureIn.height),new Vector2 (0,0));

	}
	//这个脚本也可以参数初始化
	//其初始化就是删除掉文件夹里面所有的图片
	public void makeStart()
	{
		deleteAllPictures ();
	}

	//测试用OnGUI方法
	/*
	void OnGUI()
	{ 
		if (theTexture!=null)
		{

			GUI.BeginGroup (new Rect (0, 0, 222, 222));
			GUIStyle GUIShowStyle=new GUIStyle();
			GUIShowStyle.normal.background = theTexture;
			GUI.Box (new Rect (1, 1, 112, 112), "", GUIShowStyle);
			GUI.EndGroup ();
		}
	}
    */

	//void Update ()
	//{
		//if (Input.GetKeyDown (KeyCode.Space))
			//savePicture ();
	//}
}
