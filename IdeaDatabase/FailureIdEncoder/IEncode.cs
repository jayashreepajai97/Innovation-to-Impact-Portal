using System;
using IdeaDatabase.Responses;

namespace IdeaDatabase.FailureIdEncoder
{
    public interface IEncode
    {
        FailureIdEncoderResponse FailureIdEncoder(string HpSerialNumber, string FailureCode, DateTime StartDate, DateTime TestDate);
    }
}