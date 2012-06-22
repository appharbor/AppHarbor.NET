using System;
using System.Net;
using System.Threading;

namespace AppHarbor
{
	internal static class HttpListenerExtensions
	{
		private class AsyncTask<T>
		{
			private T _result;
			private Exception _exception;
			private readonly Func<IAsyncResult, T> _endFunc;
			private readonly ManualResetEvent _completedEvent = new ManualResetEvent(false);

			public AsyncTask(Func<IAsyncResult, T> endFunc)
			{
				_endFunc = endFunc;
			}

			public void EndCallBack(IAsyncResult asyncResult)
			{
				try
				{
					_result = _endFunc(asyncResult);
				}
				catch (Exception exception)
				{
					_exception = exception;
				}
				finally
				{
					_completedEvent.Set();
				}
			}

			public T WaitResult(TimeSpan timeout)
			{
				if (!_completedEvent.WaitOne(timeout))
				{
					throw new TimeoutException();
				}

				if (_exception != null)
				{
					throw _exception;
				}

				return _result;
			}
		}

		public static HttpListenerContext GetContext(this HttpListener listener, TimeSpan timeout)
		{
			var task = new AsyncTask<HttpListenerContext>(listener.EndGetContext);
			listener.BeginGetContext(task.EndCallBack, null);
			return task.WaitResult(timeout);
		}
	}
}
