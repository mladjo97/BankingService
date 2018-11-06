﻿using BankingSectors;
using CommonStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BankingService
{

    public class ServiceProxy
    {
        
        

        public ITransactionServices TransactionProxy { get; set; }
        public ICreditServices CreditProxy { get; set; }
        public IAccountServices AccountProxy { get; set; }

        public ServiceProxy()
        {
            OpenAccountProxy();
            OpenTransactionProxy();
            OpenCreditProxy();
        }

        private void OpenTransactionProxy()
        {
            try
            {
                NetTcpBinding binding = new NetTcpBinding();
                string address = "net.tcp://localhost:9991/TransactionServices";

                binding.Security.Mode = SecurityMode.Transport;
                binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.Sign;
                binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;

                TransactionProxy = new ChannelFactory<ITransactionServices>(binding, new EndpointAddress(new Uri(address))).CreateChannel();

            }
            catch (Exception)
            {

                throw;
            }
            

            
        }
        private void OpenCreditProxy()
        {
            try
            {
                NetTcpBinding binding = new NetTcpBinding();
                string address = "net.tcp://localhost:9992/CreditServices";

                binding.Security.Mode = SecurityMode.Transport;
                binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.Sign;
                binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;

                CreditProxy = new ChannelFactory<ICreditServices>(binding, new EndpointAddress(new Uri(address))).CreateChannel();

            }
            catch (Exception)
            {

                throw;
            }
        }
        private void OpenAccountProxy()
        {
            try
            {
                NetTcpBinding binding = new NetTcpBinding();
                string address = "net.tcp://localhost:9990/AccountServices";

                binding.Security.Mode = SecurityMode.Transport;
                binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.Sign;
                binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;

                AccountProxy = new ChannelFactory<IAccountServices>(binding, new EndpointAddress(new Uri(address))).CreateChannel();

            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
