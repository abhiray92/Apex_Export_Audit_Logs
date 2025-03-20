# instrument_service.py
import json
import uuid
import websockets
import sys
from websocket import create_connection
from PopUpService import PopUpService
from Models.InstrumentInfo import InstrumentInfo

class InstrumentService:
    def get_instrument_details(self, uri):
        try:
            ws = create_connection(uri)
            print("Connected to WebSocket for instrument name.")

            request = {
                "type": "get",
                "group": "thirdPartyAPI",
                "data": {
                        "field": "information",
                        "value": True
                        },
                "messageID": str(uuid.uuid4())
                }
            json_request =  json.dumps(request)
            ws.send(json_request)


            response = ws.recv()
            
            json_response = json.loads(response)
            
            instrument_value = json_response.get('data', {}).get('value', {})
            
            if instrument_value==True:

                raise json.JSONDecodeError("Failed to decode JSON response", response, 0)
                
            instrument_info = InstrumentInfo(
            instrument_name=instrument_value.get('instrumentName', 'Unknown Instrument'),
            serial_number=instrument_value.get('serialNumber')
            )
            print(f'Instrument Name: {instrument_info.instrument_name}, Instrument Serial: {instrument_info.serial_number}')
            ws.close()
            return instrument_info

        except json.JSONDecodeError as json_ex:
            print(f"JSON error occurred: {json_ex}")
            PopUpService.show_popup("Warning", "Invalid Login to LMS Exchange. The ID/Password is incorrect or the user does not have sufficient privileges.")
            sys.exit()
        except Exception as ex:
            print(f"An unexpected error occurred: {ex}")
            PopUpService.show_popup("Warning", "An unexpected error occurred. Please try again. From Instrument")
            sys.exit()