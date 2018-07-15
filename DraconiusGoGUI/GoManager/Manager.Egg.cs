﻿using DraconiusGoGUI.Enums;
using DraconiusGoGUI.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DraconiusGoGUI.DracoManager
{
    public partial class Manager
    {
        private async Task<MethodResult> IncubateEggs()
        {
            if (!UserSettings.IncubateEggs)
            {
                LogCaller(new LoggerEventArgs("Incubating disabled", LoggerTypes.Info));

                return new MethodResult
                {
                    Message = "Incubate eggs disabled",
                    Success = true
                };
            }

            MethodResult<EggIncubator> incubatorResponse = GetIncubator();

            if (!incubatorResponse.Success)
            {
                return new MethodResult
                {
                    Message = incubatorResponse.Message,
                    Success = true
                };
            }

            PokemonData egg = Eggs.FirstOrDefault(x => String.IsNullOrEmpty(x.EggIncubatorId));

            if (egg == null)
            {
                return new MethodResult
                {
                    Message = "No egg to incubate",
                    Success = true
                };
            }

            if (!_client.LoggedIn)
            {
                MethodResult result = await AcLogin();

                if (!result.Success)
                {
                    return result;
                }
            }

            var response = await _client.ClientSession.RpcClient.SendRemoteProcedureCallAsync(new Request
            {
                RequestType = RequestType.UseItemEggIncubator,
                RequestMessage = new UseItemEggIncubatorMessage
                {
                    ItemId = incubatorResponse.Data.Id,
                    PokemonId = egg.Id
                }.ToByteString()
            });

            if (response == null)
                return new MethodResult();

            var useItemEggIncubatorResponse = UseItemEggIncubatorResponse.Parser.ParseFrom(response);

            var incitem = incubatorResponse.Data.Id;
            var _egg = egg.PokemonId;

            LogCaller(new LoggerEventArgs(String.Format("Incubating egg in {0}. Pokemon Id: {1}", incitem, _egg), LoggerTypes.Incubate));

            //TODO: Need tests
            UpdateInventory(InventoryRefresh.Eggs);
            UpdateInventory(InventoryRefresh.Incubators);

            return new MethodResult
            {
                Message = "Success",
                Success = true
            };
        }

        private MethodResult<EggIncubator> GetIncubator()
        {
            if(Incubators == null)
            {
                return new MethodResult<EggIncubator>();
            }

            EggIncubator unusedUnlimitedIncubator = Incubators.FirstOrDefault(x => x.ItemId == ItemId.ItemIncubatorBasicUnlimited && x.PokemonId == 0);

            if(unusedUnlimitedIncubator != null)
            {
                return new MethodResult<EggIncubator>
                {
                    Data = unusedUnlimitedIncubator,
                    Success = true
                };
            }

            if (!UserSettings.OnlyUnlimitedIncubator)
            {
                IEnumerable<EggIncubator> incubators = Incubators.Where(x => x.ItemId == ItemId.ItemIncubatorBasic && x.PokemonId == 0);
    
                foreach(EggIncubator incubator in incubators)
                {
                    return new MethodResult<EggIncubator>
                    {
                        Data = incubator,
                        Success = true
                    };
                }
            }

            return new MethodResult<EggIncubator>
            {
                Message = "No unused incubators"
            };
        }
    }
}
