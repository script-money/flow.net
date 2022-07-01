﻿using Flow.Net.Sdk.Core;
using Flow.Net.Sdk.Core.Cadence;
using Flow.Net.Sdk.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flow.Net.Examples.UserSignaturesExamples
{
    public class UserSignatureExample : ExampleBase
    {
        public static async Task RunAsync()
        {
            Console.WriteLine("\nRunning UserSignatureExample\n");
            await CreateFlowClientAsync();
            await Demo();
            Console.WriteLine("\nUserSignatureExample Complete\n");
        }

        private static async Task Demo()
        {
            // create the keys
            var flowAccountKeyAlice = FlowAccountKey.GenerateRandomEcdsaKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256);
            var flowAccountKeyBob = FlowAccountKey.GenerateRandomEcdsaKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256);

            // create the message that will be signed
            var aliceFlowAccount = await CreateAccountAsync(new List<FlowAccountKey> { flowAccountKeyAlice });
            var bobFlowAccount = await CreateAccountAsync(new List<FlowAccountKey> { flowAccountKeyBob });

            var toAddress = new CadenceAddress(aliceFlowAccount.Address.Address);
            var fromAddress = new CadenceAddress(bobFlowAccount.Address.Address);
            var amount = new CadenceNumber(CadenceNumberType.UInt64, "100");

            var message = Utilities.CombineByteArrays(new[]
            {
                Encoding.UTF8.GetBytes(aliceFlowAccount.Address.Address),
                Encoding.UTF8.GetBytes(bobFlowAccount.Address.Address)
            });

            var amountBytes = BitConverter.GetBytes(ulong.Parse(amount.Value));
            amountBytes = BitConverter.IsLittleEndian ? amountBytes.Reverse().ToArray() : amountBytes;

            message = Utilities.CombineByteArrays(new[]
            {
                message,
                amountBytes
            });

            // sign the message with Alice and Bob
            var aliceSigner = new Sdk.Core.Crypto.Ecdsa.Signer(flowAccountKeyAlice.PrivateKey, flowAccountKeyAlice.HashAlgorithm, flowAccountKeyAlice.SignatureAlgorithm);
            var bobSigner = new Sdk.Core.Crypto.Ecdsa.Signer(flowAccountKeyBob.PrivateKey, flowAccountKeyBob.HashAlgorithm, flowAccountKeyBob.SignatureAlgorithm);

            var aliceSignature = UserMessage.Sign(message, aliceSigner);
            var bobSignature = UserMessage.Sign(message, bobSigner);

            var publicKeys = new CadenceArray(
                new List<ICadence>
                {
                    new CadenceString(flowAccountKeyAlice.PublicKey),
                    new CadenceString(flowAccountKeyBob.PublicKey)
                });

            // each signature has half weight
            var weightAlice = new CadenceNumber(CadenceNumberType.UFix64, "0.5");
            var weightBob = new CadenceNumber(CadenceNumberType.UFix64, "0.5");

            var weights = new CadenceArray(
                new List<ICadence>
                {
                    weightAlice,
                    weightBob
                });

            var signatures = new CadenceArray(
                new List<ICadence>
                {
                    new CadenceString(aliceSignature.FromByteArrayToHex()),
                    new CadenceString(bobSignature.FromByteArrayToHex())
                });

            var script =  Utilities.ReadCadenceScript("user-signature-example");

            var response = await FlowClient.ExecuteScriptAtLatestBlockAsync(
                new FlowScript
                {
                    Script = script,
                    Arguments = new List<ICadence>
                    {
                        publicKeys,
                        weights,
                        signatures,
                        toAddress,
                        fromAddress,
                        amount,
                    }
                }
                );

            Console.WriteLine(response.As<CadenceBool>().Value
                ? "Signature verification succeeded"
                : "Signature verification failed");
        }
    }
}
