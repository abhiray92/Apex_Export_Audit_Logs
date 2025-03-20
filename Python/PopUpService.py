# popup_service.py

import tkinter as tk
from tkinter import messagebox

class PopUpService:
    @staticmethod
    def show_popup(title, message):
        root = tk.Tk()
        root.withdraw()  # Hide the main window
        if title == "Notification":
            messagebox.showinfo(title, message)
        else:
            full_message = message+"\n\nFor further assistance contact BioNX IT Team at BIONXMRLIT@msd.com"
            messagebox.showerror(title, full_message)
        root.destroy()  # Close the Tkinter window after showing the message