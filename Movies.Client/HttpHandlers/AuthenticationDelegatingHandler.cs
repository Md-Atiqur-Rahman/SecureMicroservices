namespace Movies.Client.HttpHandlers;

//এখানে getToken() মেথড ব্যবহার করা হয়নি, কারণ আমরা Delegating Handler ব্যবহার করব যা স্বয়ংক্রিয়ভাবে টোকেন সংগ্রহ করে সেট করবে।
//এটি আমাদের Http অনুরোধ ইন্টারসেপ্ট করবে এবং Identity Server থেকে টোকেন সংগ্রহ করে সেট করবে।
//এই Delegating Handler এখন প্রতিটি Http অনুরোধের আগে টোকেন সংগ্রহ করে Authorization Header এ সেট করবে।
public class AuthenticationDelegatingHandler: DelegatingHandler 
{
}
