FROM node:14.18.1-alpine
WORKDIR /app
COPY chatbotwebapp/ ./
RUN npm install
EXPOSE 3000
COPY . .
CMD ["npm", "start"]