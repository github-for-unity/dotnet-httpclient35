//
// HttpHeadersTest.cs
//
// Authors:
//	Marek Safar  <marek.safar@gmail.com>
//
// Copyright (C) 2011 Xamarin Inc (http://www.xamarin.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections;
using System.Collections.Generic;
using DotNetHttp35.Headers;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MonoTests.System.Net.Http.Headers
{
	[TestClass]
	public class HttpHeadersTest
	{
		HttpHeaders headers;

		class HttpHeadersMock : HttpHeaders
		{
		}

		[TestInitialize]
		public void Setup ()
		{
			headers = new HttpHeadersMock ();
		}

		[TestMethod]
		public void Add ()
		{
			headers.Add ("aa", "value");
			headers.Add ("aa", "value");
			headers.Add ("Expires", (string) null);
		}

		[TestMethod]
		public void Add_InvalidArguments ()
		{
			try {
				headers.Add (null, "value");
				Assert.Fail ("#1");
			} catch (ArgumentException) {
			}

			try {
				headers.Add ("", "value");
				Assert.Fail ("#2");
			} catch (ArgumentException) {
			}
		}

		[TestMethod]
		public void Clear ()
		{
			headers.Add ("aa", "value");
			headers.Clear ();
		}

		[TestMethod]
		public void GetEnumerator ()
		{
			headers.Add ("aa", "value");
			int i = 0;
			foreach (var entry in headers) {
				++i;
				Assert.AreEqual ("aa", entry.Key);
				var values = entry.Value.ToList ();
				Assert.AreEqual (1, values.Count);
				Assert.AreEqual ("value", values[0]);
			}

			Assert.AreEqual (1, i, "#10");
		}

		[TestMethod]
		public void GetValues ()
		{
			headers.Add ("aa", "v");
			headers.Add ("aa", "v");

			var r = headers.GetValues ("aa").ToList ();
			Assert.AreEqual ("v", r[0], "#1");
			Assert.AreEqual ("v", r[1], "#2");
		}

		[TestMethod]
		public void GetValues_Invalid ()
		{
			try {
				headers.GetValues (null);
				Assert.Fail ("#1");
			} catch (ArgumentException) {
			}

			try {
				headers.GetValues ("  ");
				Assert.Fail ("#2");
			} catch (FormatException) {
			}

			try {
				headers.GetValues ("x");
				Assert.Fail ("#3");
			} catch (InvalidOperationException) {
			}
		}

		[TestMethod]
		public void TryGetValuesTest ()
		{
			IEnumerable<string> headerValues;
			Assert.IsFalse (headers.TryGetValues (null, out headerValues), "#1");
			Assert.IsFalse (headers.TryGetValues ("some-name", out headerValues), "#2");
		}

		[TestMethod]
		public void ToStringTest ()
		{
			headers.Add ("aa", "v");
			headers.Add ("aa", "v");
			headers.Add ("x", "v");

			Assert.AreEqual ("aa: v, v\r\nx: v\r\n", headers.ToString ());
		}
	}
}
