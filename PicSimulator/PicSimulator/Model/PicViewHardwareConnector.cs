using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSimulator.Model
{
    public static class PicViewHardwareConnector
    {

        public static void SendData(int trisA, int portA, int trisB, int portB, SerialPort comPort)
        {
            comPort.Write(DecodeRegisterContentToCharacter(trisA));
            comPort.Write(DecodeRegisterContentToCharacter(portA));
            comPort.Write(DecodeRegisterContentToCharacter(trisB));
            comPort.Write(DecodeRegisterContentToCharacter(portB));
            comPort.Write("\r\n");
        }

        public static bool ReadData(ref int portA, ref int portB, SerialPort comPort)
        {
            var resultString = comPort.ReadExisting();
            if (resultString != string.Empty)
            {
                portA = DecodeCharacterToRegisterContent(resultString.Substring(0, 2));
                portB = DecodeCharacterToRegisterContent(resultString.Substring(2, 2));
                return true;
            }
            else
            {
                return false;
            }
        }

        private static int DecodeCharacterToRegisterContent(string character)
        {
            var firstCharacterAsInt = DecodeCharacter(character.Substring(0, 1));
            var secondCharacterAsInt = DecodeCharacter(character.Substring(1, 1));

            var highNibble = (firstCharacterAsInt & 15) << 4;
            var lowNibble = secondCharacterAsInt & 15;

            return highNibble + lowNibble;
        }

        private static string DecodeRegisterContentToCharacter(int registerContent)
        {
            var result = string.Empty;
            var highNibble = registerContent & 240;
            var lowNibble = registerContent & 15;

            var highNibbleShifted = highNibble >> 4;

            result = DecodeHexCode(highNibbleShifted + 0x30) + DecodeHexCode(lowNibble + 0x30);
            return result;
        }

        private static string DecodeHexCode(int hexCode)
        {
            switch (hexCode)
            {
                case 0x30:
                    return "0";
                case 0x31:
                    return "1";
                case 0x32:
                    return "2";
                case 0x33:
                    return "3";
                case 0x34:
                    return "4";
                case 0x35:
                    return "5";
                case 0x36:
                    return "6";
                case 0x37:
                    return "7";
                case 0x38:
                    return "8";
                case 0x39:
                    return "9";
                case 0x3A:
                    return ":";
                case 0x3B:
                    return ";";
                case 0x3C:
                    return "<";
                case 0x3D:
                    return "=";
                case 0x3E:
                    return ">";
                case 0x3F:
                    return "?";
                default:
                    return string.Empty;
            }
        }

        private static int DecodeCharacter(string character)
        {
            switch (character)
            {
                case "0":
                    return 0x30;
                case "1":
                    return 0x31;
                case "2":
                    return 0x32;
                case "3":
                    return 0x33;
                case "4":
                    return 0x34;
                case "5":
                    return 0x35;
                case "6":
                    return 0x36;
                case "7":
                    return 0x37;
                case "8":
                    return 0x38;
                case "9":
                    return 0x39;
                case ":":
                    return 0x3A;
                case ";":
                    return 0x3B;
                case "<":
                    return 0x3C;
                case "=":
                    return 0x3D;
                case ">":
                    return 0x3E;
                case "?":
                    return 0x3F;
                default:
                    return -1;
            }
        }
    }
}
