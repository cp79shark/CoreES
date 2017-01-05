using FluentAssertions.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreES.Persistence.MSSQL.Tests.Helpers
{
    public static class CustomMatchers
    {
        public static void Matches(this EventRow TableRow, string StreamId, int ExpectedEventNumber, EventData OriginalEvent, string because = "", params object[] reasonArgs)
        {
            Execute.Assertion
                .BecauseOf(because, reasonArgs)
                .ForCondition(TableRow.StreamId == StreamId)
                .FailWith("Expected retreived StreamId [{0}] to match original StreamId [{1}], but did not.", TableRow.StreamId, StreamId)
                .Then
                .BecauseOf(because, reasonArgs)
                .ForCondition(TableRow.EventId == OriginalEvent.EventId)
                .FailWith("Expected retreived EventId [{0}] to match original EventId [{1}], but did not.", TableRow.EventId, OriginalEvent.EventId)
                .Then
                .BecauseOf(because, reasonArgs)
                .ForCondition(TableRow.EventNumber == ExpectedEventNumber)
                .FailWith("Expected retreived EventNumber [{0}] to match expected EventNumber [{1}], but did not.", TableRow.EventNumber, ExpectedEventNumber)
                .Then
                .BecauseOf(because, reasonArgs)
                .ForCondition(TableRow.EventType == OriginalEvent.Type)
                .FailWith("Expected retreived EventType [{0}] to match original Type [{1}], but did not.", TableRow.EventType, OriginalEvent.Type)
                .Then
                .BecauseOf(because, reasonArgs)
                .ForCondition(TableRow.Data == OriginalEvent.Data as string)
                .FailWith("Expected retreived Data [{0}] to match original Data [{1}], but did not.", TableRow.Data, OriginalEvent.Data as string)
                .Then
                .BecauseOf(because, reasonArgs)
                .ForCondition(TableRow.Metadata == OriginalEvent.Metadata as string)
                .FailWith("Expected retreived Metadata [{0}] to match original Metadata [{1}], but did not.", TableRow.Metadata, OriginalEvent.Metadata);
        }
    }
}
