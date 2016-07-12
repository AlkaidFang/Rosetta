/*
 * Name: UpdateSystem
 * Function: 提供游戏数据更新功能
 * Author: FangJun
 * Date: 2016-06-23
 * Framework: 
 *          主要提供游戏中数据更新，分两步：
 * 				1，StreamingAsstes中的配置文件，诸如txt,lua，这种数据流文件，自己解析的这些
 * 				2，Prefab文件，需要用AB更新。
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
	public class UpdateSystem : Singleton<UpdateSystem>, Lifecycle
	{
		private const string _DownloadPath = "{}";

		private string mSavePath;		// persistive path.
		private string mFinalFilePath;	// save download path.


		public bool Init()
		{
			return true;
		}

		public void Tick(float interval)
		{
			
		}

		public void Destroy()
		{
			
		}

	}
}

