using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MongoDB.Bson;
using MongoDB.Driver;
using MVVMIDKoin.Models;
using Caliburn.Micro;

namespace IDKoin.Properties
{
    public class MongoHandler
    {
        public MongoHandler()
        {
        }
        static MongoClient client = new MongoClient("");
        static MongoDatabaseBase database = (MongoDatabaseBase)client.GetDatabase("IDKoin");
        static MongoCollectionBase<BsonDocument> collection = (MongoCollectionBase<BsonDocument>)database.GetCollection<BsonDocument>("users");
        public PersonModel Login(string username, string password)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("username", username);
            var user = collection.Find(filter).FirstOrDefault();
            if (user == null)
            {
                MessageBox.Show("User not found!","Chyba",MessageBoxButton.OK,MessageBoxImage.Error);
                return null;
                
            }
            if (user.GetElement("password").Value == password)
            {
                PersonModel person = new PersonModel(username, (int)user.GetElement("idkoin").Value, (byte[])user.GetElement("img").Value);
                return person;
            }
            else {
                MessageBox.Show("Wrong Password", "Fokof", MessageBoxButton.OK, MessageBoxImage.Hand);
                return null;
            }

        }
        public int Balance(string username)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("username", username);
            var user = collection.Find(filter).FirstOrDefault();
            if (user == null)
            {
                MessageBox.Show("User not found!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                return 0;
            }
            return (int)user.GetElement("idkoin").Value;

        }
        public void AddKoins(string username)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("username", username);
            var user = collection.Find(filter).FirstOrDefault();
            if(user == null)
            {
                MessageBox.Show(username);
                return;
            }
            var update = Builders<BsonDocument>.Update.Set("idkoin", (int)user.GetElement("idkoin").Value + 10);
            collection.UpdateOne(filter, update);
        }
        public void PayKoins(string payer,string payee,int amount)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("username", payer);
            var from = collection.Find(filter).FirstOrDefault();

            var filter2 = Builders<BsonDocument>.Filter.Eq("username", payee);
            var to = collection.Find(filter2).FirstOrDefault();
            if(from.GetElement("idkoin").Value < amount)
            {
                MessageBox.Show("Not enough idkoins!","You are poor",MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }
            if(to == null)
            {
                MessageBox.Show("User does not exist!", "U dumb?", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBoxResult result = MessageBox.Show("Are you sure?", "Pay", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (result == MessageBoxResult.No)
                return;

            var update2 = Builders<BsonDocument>.Update.Set("idkoin", (int)to.GetElement("idkoin").Value + amount);
            collection.UpdateOne(filter2, update2);

            var update = Builders<BsonDocument>.Update.Set("idkoin", (int)from.GetElement("idkoin").Value - amount);
            collection.UpdateOne(filter, update);
            MessageBox.Show("Transaction complete!","Done",MessageBoxButton.OK,MessageBoxImage.Information);
        }
        public void AddImage(string usr,byte[] imgbts)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("username", usr);
            var user = collection.Find(filter).FirstOrDefault();

            var update = Builders<BsonDocument>.Update.Set("img", imgbts);
            collection.UpdateOne(filter, update);
        }
        public byte[] GetImage(string usr)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("username", usr);
            var user = collection.Find(filter).FirstOrDefault();
            return (byte[])user.GetElement("img").Value;
        }

    }
}
