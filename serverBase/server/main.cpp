// the following code is an adaptation of the code presented on: 
// https://mariusbancila.ro/blog/2017/11/19/revisited-full-fledged-client-server-example-with-c-rest-sdk-2-10/

#include <cpprest/http_listener.h>
#include <cpprest/json.h>

#include <iostream>
#include <string>
#include <map>

#pragma comment(lib, "cpprest_2_10")

using namespace web;
using namespace web::http;
using namespace web::http::experimental::listener;
using namespace std;

int g_SessionID = 0;
map<wstring, int> Session;

void handle_get(http_request request)
{
	cout << "\nhandle GET\n";

	wstring APIuri = request.absolute_uri().to_string();
	wcout << "endpoint URI: " << APIuri << endl;
	
	
	//playerDied Endpoint
	if (wcscmp(APIuri.c_str(), L"/SLCGame311/PlayerDied/") == 0)
	{
		http_headers reqHeaders = request.headers();

		if (reqHeaders.has(L"UserID"))
		{
			json::value JSONObj = json::value::object();

			JSONObj[L"UserID"] = json::value::string(reqHeaders[L"UserID"]);

			request.reply(status_codes::OK, JSONObj);
		}
		else
		{
			request.reply(status_codes::FailedDependency, "missing headers");
		}
		
	}

	

}

void handle_post(http_request request)
{
	cout << "\nhandle POST\n";

	wstring APIuri = request.absolute_uri().to_string();
	wcout << "endpoint URI: " << APIuri << endl;

	if (wcscmp(APIuri.c_str(), L"/SLCGame311/Login") == 0)
	{
		json::value ReqBodyJSON = json::value::object();
		request.extract_json().then([&ReqBodyJSON](pplx::task<json::value> task)
			{
				ReqBodyJSON = task.get();
			}).wait();

		if (ReqBodyJSON.has_string_field(L"Name"))
		{
			utility::string_t Name = ReqBodyJSON.at(L"Name").as_string();
			if (Session[Name] != NULL)
			{
				Name.append(L", welcome back!");
			}
			else
			{
				g_SessionID++;
				Session[Name] = g_SessionID;

				Name.append(L", welcome to the server!. Since this your first time, please see the authentication token attached.");
			}			

			request.reply(status_codes::OK, Name);
		}		
		else
		{
			request.reply(status_codes::ExpectationFailed, "Missing JSON body data");
		}
	}

	request.reply(status_codes::BadRequest, "API endpoint not found");
}

int main()
{
	http_listener listener(L"http://localhost:8777/SLCGame311");

	listener.support(methods::GET, handle_get);
	listener.support(methods::POST, handle_post);
	
	try
	{
		listener.open().then([&listener]() 
		{
			wcout << ("\nlistening::\n") << listener.uri().to_string().c_str(); 
		})
		.wait();
	}
	catch (exception const & e)
	{
		wcout << e.what() << endl;
	}

	// infinite while loop to ensure our application continues to run and doesn't reach the end
	while (true);

	return 0;
}
