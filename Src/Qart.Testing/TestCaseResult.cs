﻿using System;
using System.Xml.Linq;

namespace Qart.Testing
{
    public class TestCaseResult
    {
        public TestCase TestCase { get; private set; }
        public Exception Exception { get; private set; }
        public XDocument Description { get; set; }

        public TestCaseResult(TestCase testCase)
        {
            TestCase = testCase;
        }

        public void MarkAsFailed(Exception ex)
        {
            Exception = ex;
        }
    }
}
