# main.py
from InstrumentService import InstrumentService
from LogService import LogService
from datetime import datetime

def main():
    uri = "ws://192.168.1.2:4000"
    instrument_service = InstrumentService()

    instrument_info = instrument_service.get_instrument_details(uri)
    #print(instrument_info)
    log_file_path = f"C:\\software\\auditlogs_{instrument_info.instrument_name}_{instrument_info.serial_number}_{datetime.utcnow().strftime('%d-%b-%Y')}.LOG"
    
    log_service = LogService()
    log_service.export_audit_logs(instrument_info.instrument_name, instrument_info.serial_number, log_file_path, uri)

if __name__ == "__main__":
    main()