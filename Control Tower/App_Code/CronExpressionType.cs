using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ServiceInterface
/// </summary>

public enum CronExpressionType
{
    EveryNMinutes,
    EveryNHours,
    EveryDayAt,
    EveryNDaysAt,
    EveryWeekDay,
    EverySpecificWeekDayAt,
    EverySpecificDayEveryNMonthAt,
    EverySpecificSeqWeekDayEveryNMonthAt,
    EverySpecificDayOfMonthAt,
    EverySpecificSeqWeekDayOfMonthAt,
    EverySpecificDayMonthAt
}