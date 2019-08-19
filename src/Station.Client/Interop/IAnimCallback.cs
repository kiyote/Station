using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Station.Client.Interop {
	public interface IAnimCallback {

		Task RenderFrame( float interval );
	}
}
