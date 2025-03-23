using System;
using System.Collections.Generic;

namespace RVO.Arithmetic.Optimized
{
	// 资源池(基类)
	public abstract class ResourcePool
	{
		protected bool fresh = true;

		// 为所有资源池对象提供一个引用列表
		protected static List<ResourcePool> resourcePoolReferences = new List<ResourcePool>();

		// 清理所有
		public static void CleanUpAll()
		{
			int i = 0;
			int count = ResourcePool.resourcePoolReferences.Count;
			while (i < count)
			{
				ResourcePool.resourcePoolReferences[i].ResetResourcePool();
				i++;
			}
			ResourcePool.resourcePoolReferences.Clear();
		}

		// 重置
		public abstract void ResetResourcePool();
	}

	// 资源池
	public class ResourcePool<T> : ResourcePool
	{
		protected Stack<T> stack = new Stack<T>(10);

		#region 属性
		// 元素的数量
		public int Count
		{
			get
			{
				return this.stack.Count;
			}
		}

		// 重置
		public override void ResetResourcePool()
		{
			this.stack.Clear();
			this.fresh = true;
		}

		// 归还元素
		public void GiveBack(T obj)
		{
			this.stack.Push(obj);
		}

		// 获取元素
		public T GetNew()
		{
			bool fresh = this.fresh;
			if (fresh)
			{
				ResourcePool.resourcePoolReferences.Add(this);
				this.fresh = false;
			}
			bool flag = this.stack.Count == 0;
			if (flag) // 如果没有元素
			{
				this.stack.Push(this.NewInstance());
			}
			T t = this.stack.Pop();
			bool flag2 = t is ResourcePoolItem;
			if (flag2) // 如果是ResourcePoolItem类型，需要执行清理
			{
				((ResourcePoolItem)((object)t)).CleanUp();
			}
			return t;
		}
		#endregion 属性

		// 元素实例化
		protected virtual T NewInstance()
		{
			return Activator.CreateInstance<T>();
		}
	}
}
