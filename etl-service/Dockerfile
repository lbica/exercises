FROM python:3.9
WORKDIR /app
COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt

COPY app.py .
COPY data /app/data/


EXPOSE 5000
CMD ["python", "app.py"]
