using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Station.Client {
	public class Font {

		public Font(string name, int size) {
			Name = name;
			Size = size;
			Value = $"{size}px {name}";
		}

		public string Name { get; }

		public int Size { get; }

		public string Value { get; }
	}
}
