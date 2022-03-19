using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

using System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
public sealed class MethodProperty : Attribute
{

	public MethodProperty() : base()
	{
	}

	public bool Export { get; set; }

	public string AliasName { get; set; }
	//暂不支持

	public CallingConvention CallingConvention { get; set; }
	//暂不支持

}
