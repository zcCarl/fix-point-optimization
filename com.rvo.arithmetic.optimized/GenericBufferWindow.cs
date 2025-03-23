using System;

namespace RVO.Arithmetic.Optimized
{
	// 通用缓冲窗口(循环列表结构)
	public class GenericBufferWindow<T>
	{
		public delegate T NewInstance(); // 实例

		public T[] buffer; // 缓冲区

		public int size; // 大小

		public int currentIndex; // 当前索引

		#region 构造函数
		public GenericBufferWindow(int size)
		{
			this.size = size;
			this.currentIndex = 0;
			this.buffer = new T[size];
			for (int i = 0; i < size; i++)
			{
				this.buffer[i] = Activator.CreateInstance<T>();
			}
		}

		public GenericBufferWindow(int size, GenericBufferWindow<T>.NewInstance NewInstance)
		{
			this.size = size;
			this.currentIndex = 0;
			this.buffer = new T[size];
			for (int i = 0; i < size; i++)
			{
				this.buffer[i] = NewInstance();
			}
		}
		#endregion 构造函数

		#region 方法
		// 重置大小
		public void Resize(int newSize)
		{
			bool flag = newSize == this.size;
			if (!flag) // 如果大小不同，需要重新分配
			{
				T[] array = new T[newSize]; // 新缓冲区
				int num = newSize - this.size; // 差值
				bool flag2 = newSize > this.size;
				if (flag2) // 增加
				{
					for (int i = 0; i < this.size; i++)
					{
						bool flag3 = i < this.currentIndex;
						if (flag3)
						{
							array[i] = this.buffer[i];
						}
						else // 已经超出currentIndex的数据，移动到新缓冲区末尾 // 移动
						{
							array[i + num] = this.buffer[i];
						}
					}
					for (int j = 0; j < num; j++) // 在新缓冲区末尾填充空实例
					{
						array[this.currentIndex + j] = Activator.CreateInstance<T>();
					}
				}
				else // 减少
				{
					for (int k = 0; k < newSize; k++)
					{
						bool flag4 = k < this.currentIndex;
						if (flag4)
						{
							array[k] = this.buffer[k];
						}
						else
						{
							array[k] = this.buffer[k - num];
						}
					}
					this.currentIndex %= newSize;
				}
				this.buffer = array;
				this.size = newSize;
			}
		}

		// 设置元素
		public void Set(T instance)
		{
			this.buffer[this.currentIndex] = instance;
		}

		// 获取前一个元素
		public T Previous()
		{
			int num = this.currentIndex - 1;
			bool flag = num < 0;
			if (flag) // 如果没有前一个元素，返回最后一个
			{
				num = this.size - 1;
			}
			return this.buffer[num];
		}

		// 获取当前元素
		public T Current()
		{
			return this.buffer[this.currentIndex];
		}

		// 移动到下一个元素
		public void MoveNext()
		{
			this.currentIndex = (this.currentIndex + 1) % this.size;
		}
		#endregion 方法
	}
}
