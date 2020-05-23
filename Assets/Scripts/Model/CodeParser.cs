using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class CodeParser
{
    public CodeParser()
    {
        string json = @"{
                            ""code"": [
                            {
                                    ""block"": [
                                    {
                                        ""attr"": {
                                            ""@_type"": ""controls_if""
                                        },
                                    ""value"": [
                                    {
                                            ""attr"": {
                                                ""@_name"": ""IF0""
                                            },
                                        ""block"": [
                                        {
                                                ""attr"": {
                                                    ""@_type"": ""logic_boolean""
                                                },
                                            ""field"": [
                                            {
                                                    ""#text"": ""TRUE"",
                                                ""attr"": {
                                                        ""@_name"": ""BOOL""
                                                }
                                                }
                                            ]
                                        }
                                        ]
                                    }
                                    ],
                                    ""statement"": [
                                    {
                                            ""attr"": {
                                                ""@_name"": ""DO0""
                                            },
                                        ""block"": [
                                        {
                                                ""attr"": {
                                                    ""@_type"": ""jump_arpb2""
                                                }
                                            }
                                        ]
                                    }
                                    ]
                                }
                                ]
                            }
                            ]
                        }";
        List<object> code = JsonConvert.DeserializeObject<List<object>>(json);
        Console.WriteLine(code);
    }
}
