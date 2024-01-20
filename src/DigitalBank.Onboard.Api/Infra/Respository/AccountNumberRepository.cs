using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalBank.Onboard.Api.Infra.Respository
{
    public interface IAccountNumberRepository
    {      
        int GetNextAccountNumberAvailable(int agency);  
        void CreateAccountNumberPool(int agency);
    }   

    public class AccountNumberRepository
    {        
        public int GetNextAccountNumberAvailable(int agency)
        {
            return new Random().Next(100000, 999999);
            //Logic dumb for test. The right approach is to get the next available account number 
            //from Redis by agency
        }     

        public void CreateAccountNumberPool(int agency)
        {
            //Logic dumb for test. The right approach is to create a new account number pool in Redis
            //by agency
        }   
    }
}