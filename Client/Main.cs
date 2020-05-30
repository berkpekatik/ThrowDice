using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core;
using System.Threading;

namespace Client
{
    public class Main : BaseScript
    {
        private static Ped p = Game.Player.Character;
        private static int? randomNumber;
        private static bool gameAllreadyStart = false;
        private static List<string> OldDiceList = new List<string>();
        public Main()
        {
            RegisterCommand("zar", new Action(Dice), false);
            RegisterCommand("zarliste", new Action(OldDices), false); //chat commands

            EventHandlers["Dice:ZarAt"] += new Action<int, int>(DiceCustom);
            EventHandlers["Dice:Zar"] += new Action(Dice);
            EventHandlers["Dice:ZarListe"] += new Action(OldDices); //Global event handlers 
            Tick += OnTick;
        }

        private void OldDices()
        {
            if (gameAllreadyStart)
            {
                ChatSendMessage("Zar", "Zar atarken listeye bakamazsın!");
                return;
            }

            if (OldDiceList.Count > 0)
            {
                ChatSendMessage("Eski Atılan Zarlar: ", String.Join(", ", OldDiceList));
            }
            else
            {
                ChatSendMessage("Zar", "Daha önce hiç zar atmadın!");
            }
        }

        private async Task OnTick()
        {
            if (randomNumber.HasValue && randomNumber > 0)
            {
                Draw3DText(p.Position, "Atılan iki zar'dan: " + randomNumber.ToString() + " geldi", 255, 255, 255, 0.6f);
            }
            else if (randomNumber.HasValue && randomNumber == 0)
            {
                Draw3DText(p.Position, "Zarlar atılıyor..", 255, 255, 255, 0.6f);
            }
        }

        private async void DiceCustom(int start, int end)
        {
            if (p.IsInVehicle() || p.IsDead || p.IsCuffed || p.IsClimbing || p.IsInHeli || p.IsInFlyingVehicle || p.IsJumping || p.IsFalling || p.IsDiving || p.IsSwimming)
            {
                ChatSendMessage("Zar", "Şuanda Zar Atamazsın!");
                return;
            }
            if (gameAllreadyStart)
            {
                ChatSendMessage("Zar", "Şuan zaten zar atıyorsun!");
                return;
            }
            gameAllreadyStart = true;
            randomNumber = 0;
            var random = new Random();
            var newRandom = random.Next(start, end);
            OldDiceList.Add(newRandom.ToString());
            p.Task.PlayAnimation("anim@mp_player_intcelebrationmale@wank", "wank", 8.0f, -1, AnimationFlags.None);
            await Delay(3000);
            p.Task.ClearAll();
            randomNumber = newRandom;
            await Delay(3000);
            randomNumber = null;
            gameAllreadyStart = false;
        }
        private async void Dice()
        {
            if (p.IsInVehicle() || p.IsDead || p.IsCuffed || p.IsClimbing || p.IsInHeli || p.IsInFlyingVehicle || p.IsJumping || p.IsFalling || p.IsDiving)
            {
                ChatSendMessage("Zar", "Şuan Zar Atamazsın!");
                return;
            }
            if (gameAllreadyStart)
            {
                ChatSendMessage("Zar", "Şuan zaten zar atıyorsun!");
                return;
            }
            gameAllreadyStart = true;
            randomNumber = 0;
            var random = new Random();
            var newRandom = random.Next(2, 12);
            OldDiceList.Add(newRandom.ToString());
            p.Task.PlayAnimation("anim@mp_player_intcelebrationmale@wank", "wank", 8.0f, -1, AnimationFlags.None);
            await Delay(3000);
            p.Task.ClearAll();
            randomNumber = newRandom;
            await Delay(3000);
            randomNumber = null;
            gameAllreadyStart = false;
        }
        private void Draw3DText(Vector3 vector, string name, int r, int g, int b, float zoom = 0f)
        {
            var coords = GetEntityCoords(PlayerPedId(), false);
            if (Vdist2(coords.X, coords.Y, coords.Z, vector.X, vector.Y, vector.Z + 1) < 1000F)
            {
                float x = 0f, y = 0f;
                var onscreen = World3dToScreen2d(vector.X, vector.Y, vector.Z + 1, ref x, ref y);
                var p = GetGameplayCamCoord();
                var distance = GetDistanceBetweenCoords(p.X, p.Y, p.Z, vector.X, vector.Y, vector.Z + 1, false);
                var scale = (1f / distance) * (2f);
                var fov = (1f / GetGameplayCamFov()) * 75f;
                scale = scale * fov;
                if (zoom != 0) scale = scale * zoom;
                if (onscreen)
                {
                    SetTextScale(0, scale);
                    SetTextFont(0);
                    SetTextProportional(true);
                    SetTextColour(r, g, b, 255);
                    SetTextDropshadow(0, 0, 0, 0, 255);
                    SetTextEdge(2, 0, 0, 0, 150);
                    SetTextOutline();
                    SetTextEntry("STRING");
                    SetTextCentre(true);
                    AddTextComponentString(name);
                    DrawText(x, y);
                }
            }
        }

        private void ChatSendMessage(string title, string text)
        {
            TriggerEvent("chat:addMessage", new
            {
                color = new[] { 255, 0, 0 },
                multiline = true,
                args = new[] { title, text }
            });
        }
    }
}
