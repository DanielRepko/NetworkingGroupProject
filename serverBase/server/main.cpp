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

int highScore = 0;

void handle_get(http_request request)
{
	cout << "\nhandle GET\n";

	wstring APIuri = request.absolute_uri().to_string();
	wcout << "endpoint URI: " << APIuri << endl;

	//GetHighScore endpoint
	if (wcscmp(APIuri.c_str(), L"/SLCGame311/GetHighScore") == 0)
	{
		json::value JSONObj = json::value::object();

		http_headers reqHeaders = request.headers();

		//checks for neccesary headers
		if (reqHeaders.has(L"TokenID") && reqHeaders.has(L"Name"))
		{
			//check if TokenID in header matches token stored on the server
			if(reqHeaders[L"TokenID"] == to_wstring(Session[reqHeaders[L"Name"]]))
			{
				//return the current high score
				JSONObj[L"Highscore"] = highScore;
				request.reply(status_codes::OK, JSONObj);
			}

			request.reply(status_codes::BadRequest, "Authentication Failed");
		}

		request.reply(status_codes::FailedDependency, "Missing headers");
	}
	

}

void handle_post(http_request request)
{
	cout << "\nhandle POST\n";

	wstring APIuri = request.absolute_uri().to_string();
	wcout << "endpoint URI: " << APIuri << endl;

	//Login endpoint
	if (wcscmp(APIuri.c_str(), L"/SLCGame311/Login") == 0)
	{
		json::value ReqBodyJSON = json::value::object();
		request.extract_json().then([&ReqBodyJSON](pplx::task<json::value> task)
			{
				ReqBodyJSON = task.get();
			}).wait();

		int id = NULL;

		//check for required body
		if (ReqBodyJSON.has_string_field(L"Name"))
		{
			utility::string_t Name = ReqBodyJSON.at(L"Name").as_string();

			//new player login
			if (Session[Name] != NULL)
			{
				id = Session[Name];
			}
			else //returning player login
			{
				g_SessionID++;
				id = g_SessionID;
				Session[Name] = g_SessionID;
			}

			json::value JSONObj = json::value::object();
			JSONObj[L"TokenID"] = id;

			request.reply(status_codes::OK, JSONObj);
		}		
		else
		{
			request.reply(status_codes::ExpectationFailed, "Missing JSON body data");
		}
	}

	//SetScore endpoint
	if (wcscmp(APIuri.c_str(), L"/SLCGame311/SetScore") == 0)
	{
		json::value ReqBodyJSON = json::value::object();
		request.extract_json().then([&ReqBodyJSON](pplx::task<json::value> task)
		{
			ReqBodyJSON = task.get();
		}).wait();

		//check for required body
		if (ReqBodyJSON.has_integer_field(L"Score"))
		{
			http_headers reqHeaders = request.headers();
			int score = ReqBodyJSON.at(L"Score").as_integer();

			json::value JSONObj = json::value::object();

			//check for required headers
			if (reqHeaders.has(L"TokenID") && reqHeaders.has(L"Name"))
			{
				//check if player acheived a new high score
				if (score > highScore)
				{
					highScore = score;
					JSONObj[L"IsHighScore"] = json::value::boolean(true);
				}
				else
				{
					JSONObj[L"IsHighScore"] = json::value::boolean(false);
				}
				//reply with JSON object notifying player if they got a high score
				request.reply(status_codes::OK, JSONObj);
			}
			request.reply(status_codes::FailedDependency, "Missing headers");
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
