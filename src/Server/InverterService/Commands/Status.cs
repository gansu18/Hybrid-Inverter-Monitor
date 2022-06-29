﻿using InverterMon.Shared.Models;

namespace InverterMon.Server.InverterService.Commands;

internal class Status : CommandBase<InverterStatus>
{
    protected override string Command => "QPIGS";

    public async Task<bool> Update()
    {
        await Task.Delay(1000);

        if (ReadFailed)
            return false;

        string[]? parts = RawResponse[1..].Split(' ', StringSplitOptions.RemoveEmptyEntries);

        Data.GridVoltage = decimal.Parse(parts[0]);
        Data.GridFrequency = decimal.Parse(parts[1]);
        Data.OutputVoltage = decimal.Parse(parts[2]);
        Data.OutputFrequency = decimal.Parse(parts[3]);
        Data.LoadVA = int.Parse(parts[4]);
        Data.LoadWatts = int.Parse(parts[5]);
        Data.LoadPercentage = decimal.Parse(parts[6]);
        Data.BusVoltage = decimal.Parse(parts[7]);
        Data.BatteryVoltage = decimal.Parse(parts[8]);
        Data.BatteryChargeCurrent = int.Parse(parts[9]);
        Data.BatteryCapacity = int.Parse(parts[10]);
        Data.HeatSinkTemperature = int.Parse(parts[11]);
        Data.PVInputCurrent = decimal.Parse(parts[12]);
        Data.PVInputVoltage = decimal.Parse(parts[13]);
        Data.SCCVoltage = decimal.Parse(parts[14]);
        Data.BatteryDischargeCurrent = int.Parse(parts[15]);
        Data.PVOrACFeed = parts[16][0];
        Data.LoadOn = parts[16][3];
        Data.SCCOn = parts[16][1];
        Data.ACChargeOn = parts[16][2];
        Data.PVInputWatt = int.Parse(parts[19]);

        return true;
    }
}
