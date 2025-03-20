# log_service.py

import json
import os
import sys
import websocket
import uuid
from datetime import datetime
from datetime import timedelta
from PopUpService import PopUpService
from websocket import create_connection


class LogService:
    def export_audit_logs(self, instrument_name, serial_number, log_file_path, uri):
        try:
            ws = create_connection(uri)
            print("Connected to WebSocket for audit logs.")

            request = {
                    "type": "get",
                    "group": "thirdPartyAPI",
                    "data": {
                        "field": "auditTrails",
                        "value": {
                            "startDateTime": datetime.utcnow().isoformat() + "Z",
                            "endDateTime": (datetime.utcnow() + timedelta(days=1)).isoformat() + "Z"
                        }
                    },
                    "messageID": str(uuid.uuid4())
                }

            json_request = json.dumps(request)
            ws.send(json_request)

            response = ws.recv()
            audit_response = json.loads(response)
            
            self.write_logs_to_file(audit_response.get('data', {}).get('value'), instrument_name, serial_number, log_file_path)

            ws.close()

        except json.JSONDecodeError as json_ex:
            print(f"JSON error occurred: {json_ex}")
            PopUpService.show_popup("Warning", "Invalid Login to LMS Exchange. The ID/Password is incorrect or the user does not have sufficient privileges.")
            sys.exit()
        
        except websocket.WebSocketException as web_socket_ex:
            print(f"WebSocket error occurred: {web_socket_ex}")
            PopUpService.show_popup("WebSocket connection failed.", "Warning")
            sys.exit()

        except Exception as ex:
            print(f"An unexpected error occurred: {ex}")
            PopUpService.show_popup("Warning", "An unexpected error occurred. Please try again.")
            sys.exit()

    def write_logs_to_file(self, log_data, instrument_name, serial_number, log_file_path):
        if log_data:
            log_content = []
            log_content.append(f"Instrument Name: {instrument_name}")
            log_content.append(f"Serial Number: {serial_number}\n")
            
            #Write log headers
            log_content.append(f"{'Instrument Name':<25}{'Timestamp':<30}{'User':<20}{'Message'}")

            for log in log_data:
                timestamp = datetime.fromisoformat(log['timestamp']).strftime("%d-%b-%Y %H:%M:%S")
                user = log.get('user', 'N/A')
                message = log.get('message', 'N/A')

                log_content.append(f"{instrument_name:<25}{timestamp:<25}{user:<20}{message}")

            with open(log_file_path, 'w') as log_file:
                log_file.write('\n'.join(log_content))
            print(f"Logs exported to {log_file_path}")
            PopUpService.show_popup("Notification", f"Logs exported to {log_file_path}")
        else:
            print("No log data found.")
            PopUpService.show_popup("Notification", "No Log data found!")
