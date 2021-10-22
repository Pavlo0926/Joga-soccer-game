using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebSocketConstant : MonoBehaviour
{
	/*************************************************** NODE APIs ***********************************************/
    // Production URL
    // public const string BASE_URL = "https://jiweman.com:3000";
    // public const string IMAGE_BASE_URL = "https://jiweman.com";
    // Development URL
    public const string BASE_URL = "https://139.59.45.231:3000";
    public const string IMAGE_BASE_URL = "https://jiweman.com";
    // public const string BASE_URL = "https://159.89.172.140:3000";
    
    
    // public const string BASE_URL = "http://192.168.0.45:3000";
    //Registration
    public const string GET_REGISTRATION = BASE_URL + "/auth/registerWithJieman";
    public const string GET_LOGINWITHFB = BASE_URL + "/auth/loginWithFacebook";  // Login with facebook
    public const string GET_LOGINJIWEMAN = BASE_URL + "/auth/loginwithJiweman";
    public const string GET_GUESREGISTRATION = BASE_URL + "/auth/loginAsGuest";
    public const string GET_PLAYERPROFILE = BASE_URL + "/playerProfile"; // Get Player profile data
    public const string GET_ALLFORMATION = BASE_URL + "/formations";
    public const string GET_ALLFRIENDLIST = BASE_URL + "/PlayWithFriend/getAllFriends";
    public const string GET_SEARCHFRIEND = BASE_URL + "/PlayWithFriend/searchAllFriends";
    public const string GET_ALLPLAYERS = BASE_URL + "/players";
    public const string GET_SEARCHALLPLAYERS = BASE_URL + "/PlayWithFriend/searchAllPlayers";
    public const string GET_ADDFRIEND = BASE_URL + "/PlayWithFriend/addPlayerFriendList";
    public const string GET_DELETEFRIEND = BASE_URL + "/PlayWithFriend/deletePlayerFriendList";
    public const string GET_CHALLENGEFRIEND = BASE_URL + "/PlayWithFriend/challengeFriend";
    public const string GET_CHALLENGESTATUS = BASE_URL + "/PlayWithFriend/challengeStatus";
    public const string GET_SAVE_DEVICE_TOKEN = BASE_URL + "/PlayWithFriend/saveDeviceToken";
    public const string GET_LEADERBOARD = BASE_URL + "/leaderboard";
    public const string GET_LEAGUE = BASE_URL + "/league";
    public const string GET_FORGRT_PASSWORD = BASE_URL + "/auth/forgotpassword";
    public const string GET_POINTS_INFO = BASE_URL + "/pointsInfo";



	/*************************************************** JAVA APIs ***********************************************/
/*
    //Crest Name
    public const string GET_UPDATECRESTNAME = "http://52.15.74.142:8080/fingersoccer/users/updateUserCrest";
    //All Stadium
    public const string GET_ALLSTADIUM = "http://52.15.74.142:8080/fingersoccer/Stadium/getAllStadiumByUserId";
    public const string GET_SELECTSTADIUM = "http://52.15.74.142:8080/fingersoccer/Stadium/selectStadiumByUser";
    //Brnading
    public const string GET_BRANDS_BY_BG = "http://52.15.74.142:8080/fingersoccer/brand/getAllImagesByBrand";
    public const string GET_BRANDS_BY_LOGO = "http://52.15.74.142:8080/fingersoccer/brand/getAllBrands";
    //Stats
    public const string GET_ALLTEAM = "http://52.15.74.142:8080/fingersoccer/team/getAllTeams";//java
    public const string GET_DISK_BY_DISKID = "http://52.15.74.142:8080/fingersoccer/team/getDiskByDiskId";
    public const string GET_UPDATE_DISK = "http://52.15.74.142:8080/fingersoccer/team/updateDiskData";
    // public const string RESET_USER_DISK_STATS = "http://52.15.74.142:8080/fingersoccer/team/updateDiskData";
    // public const string GET_REGISTRATION = "http://52.15.74.142:8080/fingersoccer/users/registerUser"; //JAVA
    // public const string GET_LOGINJIWEMAN = "http://52.15.74.142:8080/fingersoccer/users/loginUser";   //JAVA
    // play with Friends
    // public const string GET_ALLFORMATION = "http://52.15.74.142:8080/fingersoccer/formation/getAllFormationByUserId"; //JAVA
    // public const string GET_ALLFRIENDLIST = "http://52.15.74.142:8080/fingersoccer/userFriendMapping/getAllFriendList"; //JAVA
    // public const string GET_FINDFRIEND = "http://52.15.74.142:8080/fingersoccer/userFriendMapping/findFriends"; // JAVA
    // public const string GET_ADDFRIEND = "http://52.15.74.142:8080/fingersoccer/userFriendMapping/addFriend";   //java
    public const string GET_REMOVEFRIEND = "http://52.15.74.142:8080/fingersoccer/userFriendMapping/removeFriend";
    public const string GET_ALLCHALLENGE = "http://52.15.74.142:8080/fingersoccer/userFriendMapping/getAllChallenges";
    public const string GET_SENDCHALLENGE = "http://52.15.74.142:8080/fingersoccer/userFriendMapping/sendChallenge";
    public const string GET_ACCEPTCHALLENGE = "http://52.15.74.142:8080/fingersoccer/userFriendMapping/acceptChallenge";
    //Betting
    public const string GET_ALLBETTINGCOMPANY = "http://52.15.74.142:8080/fingersoccer/BettingCompany/getAllCompany";
    public const string GET_SELECTBETTINGCOMPANYBYUSER = "http://52.15.74.142:8080/fingersoccer/BettingCompany/selectBettingCompanyByUser";
    public const string GET_UPDATEUSERBETTINGCOMPANYRP = "http://52.15.74.142:8080/fingersoccer/BettingCompany/updateUserBettingCompanyRp";
    //SAVE FCM Token
    public const string GET_SAVEDEVICETOKEN = "http://52.15.74.142:8080/fingersoccer/users/saveDeviceToken";
    //Achievement
    public const string GET_UNLOCKEDACHIEVEMENT = "http://52.15.74.142:8080/fingersoccer/Achievement/unlockAchievement";
    public const string GET_ALLACHIEVEMENT = "http://52.15.74.142:8080/fingersoccer/Achievement/getAllAchievement";
    //RealMoneyOneOnOne without Brand sponsor
    public const string GET_REALMONEYONENONE = "http://52.15.74.142:8080/fingersoccer/OnGoingBettingMatch/RealMoneyOneOnOne";
    public const string GET_REALMONEYTOURNAMENT = "http://52.15.74.142:8080/fingersoccer/OnGoingBettingMatch/RealMoneyTournament";
    public const string GET_TOURNAMENTMATCHID = "http://52.15.74.142:8080/fingersoccer/OnGoingBettingMatch/getTournamentMatchId";
    public const string GET_ALLTOURNAMENTROOMID = "http://52.15.74.142:8080/fingersoccer/OnGoingBettingMatch/getAllTournamentRoom";
    public const string GET_BRANDSPONSORREALMONEYONENONE = "http://52.15.74.142:8080/fingersoccer/OnGoingBettingMatch/brandSponsoredRealMoneyOneOnOne";
    public const string GET_BRANDSPONSORREALMONEYTOURNAMENTSTART = "http://52.15.74.142:8080/fingersoccer/BrandSponserTournament/brandSponsorTournamentStart";
    public const string GET_BRANDSPONSORREALMONEYTOURNAMENTWINNER = "http://52.15.74.142:8080/fingersoccer/BrandSponserTournament/brandSponserTournamentWinner";
    public const string GET_BRANDSPONSORREALMONEYTOURNAMENTEND = "http://52.15.74.142:8080/fingersoccer/BrandSponserTournament/brandSponserTournamentend";
    public const string GET_BRANDSPONSORREALMONEYTOURNAMENTTERMINATE = "http://52.15.74.142:8080/fingersoccer/BrandSponserTournament/brandSponserTournamnetTerminate";
    public const string GET_BRANDSPONSORREALMONEYTOURNAMENTWINNERDETILS = "http://52.15.74.142:8080/fingersoccer/SponserTournamentWinners/sponserTournamentWinners";
*/
}