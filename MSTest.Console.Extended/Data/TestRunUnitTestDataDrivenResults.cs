﻿// <copyright file="TestRunUnitTestDataDrivenResults.cs" company="Automate The Planet Ltd.">
// Copyright 2016 Automate The Planet Ltd.
// Licensed under the Apache License, Version 2.0 (the "License");
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// <author>Anton Angelov</author>
// <site>http://automatetheplanet.com/</site>
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace MSTest.Console.Extended.Data
{
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
    public partial class TestRunUnitTestDataDrivenResults
    {
        private TestRunUnitTestResultOutput outputField;

        private string executionIdField;

        private string testIdField;

        private string testNameField;

        private string computerNameField;

        private System.TimeSpan durationField;

        private System.DateTime startTimeField;

        private System.DateTime endTimeField;

        private string testTypeField;

        private string outcomeField;

        private string testListIdField;

        private string relativeResultsDirectoryField;
    
        public TestRunUnitTestResultOutput Output
        {
            get
            {
                return this.outputField;
            }
            set
            {
                this.outputField = value;
            }
        }
    
        [XmlAttributeAttribute]
        public string executionId
        {
            get
            {
                return this.executionIdField;
            }
            set
            {
                this.executionIdField = value;
            }
        }
    
        [XmlAttributeAttribute]
        public string testId
        {
            get
            {
                return this.testIdField;
            }
            set
            {
                this.testIdField = value;
            }
        }
    
        [XmlAttributeAttribute]
        public string testName
        {
            get
            {
                return this.testNameField;
            }
            set
            {
                this.testNameField = value;
            }
        }
    
        [XmlAttributeAttribute]
        public string computerName
        {
            get
            {
                return this.computerNameField;
            }
            set
            {
                this.computerNameField = value;
            }
        }

        [XmlIgnore]
        public TimeSpan duration
        {
            get
            {
                return this.durationField;
            }
            set
            {
                this.durationField = value;
            }
        }

        // XmlSerializer does not support TimeSpan, so use this property for 
        // serialization instead.
        [Browsable(false)]
        [XmlAttributeAttribute(DataType = "duration", AttributeName = "duration")]
        public string DurationString
        {
            get
            {
                return this.duration.ToString();
            }
            set
            {
                this.duration = string.IsNullOrEmpty(value) ? TimeSpan.Zero : TimeSpan.Parse(value);
            }
        }
    
        [XmlAttributeAttribute]
        public System.DateTime startTime
        {
            get
            {
                return this.startTimeField;
            }
            set
            {
                this.startTimeField = value;
            }
        }
    
        [XmlAttributeAttribute]
        public System.DateTime endTime
        {
            get
            {
                return this.endTimeField;
            }
            set
            {
                this.endTimeField = value;
            }
        }
    
        [XmlAttributeAttribute]
        public string testType
        {
            get
            {
                return this.testTypeField;
            }
            set
            {
                this.testTypeField = value;
            }
        }
    
        [XmlAttributeAttribute]
        public string outcome
        {
            get
            {
                return this.outcomeField;
            }
            set
            {
                this.outcomeField = value;
            }
        }
    
        [XmlAttributeAttribute]
        public string testListId
        {
            get
            {
                return this.testListIdField;
            }
            set
            {
                this.testListIdField = value;
            }
        }

        [XmlAttributeAttribute]
        public string relativeResultsDirectory
        {
            get
            {
                return this.relativeResultsDirectoryField;
            }
            set
            {
                this.relativeResultsDirectoryField = value;
            }
        }
    }
}