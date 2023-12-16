﻿namespace InnoGotchiWebAPI.Exceptions
{
    public class InnoGotchiPetNotFoundException : InnoGotchiException
    {
        public InnoGotchiPetNotFoundException(string? message = default) : base(message, 404) { }
    }
}