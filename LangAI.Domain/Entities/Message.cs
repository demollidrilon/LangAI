﻿namespace LangAI.Domain.Entities;
public class Message
{
    public string Role { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}