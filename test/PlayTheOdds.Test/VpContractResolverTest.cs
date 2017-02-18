using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PlayTheOdds.Models;
using PlayTheOdds.VPGame;
using PlayTheOdds.VPGame.Json;

namespace PlayTheOdds.Test
{
    [TestClass]
    public class VpContractResolverTest
    {
        [TestMethod]
        public void DeserializeMatches()
        {
            using (var data = new JsonTextReader(new StreamReader(File.OpenRead(@"E:\Projects\Web\PlayTheOdds\test\PlayTheOdds.Test\matches.json"))))
            {
                var matches = VpJsonSerializer.Instance.Deserialize<Envelope<Match>>(data);
            }
        }

        [TestMethod]
        public void DeserializeWagers()
        {
            using (var data = new JsonTextReader(new StreamReader(File.OpenRead(@"E:\Projects\Web\PlayTheOdds\test\PlayTheOdds.Test\wagers.json"))))
            {
                var wagers = VpJsonSerializer.Instance.Deserialize<Envelope<Wager>>(data);
            }
        }
    }
}