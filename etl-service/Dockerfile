FROM python:3.10

WORKDIR /app

COPY requirements.txt .
# RUN pip install --no-cache-dir -r requirements.txt

COPY main.py .
COPY data /app/data/

WORKDIR /errors

# CMD ["python", "main.py"]

CMD ["tail", "-f", "/dev/null"]
