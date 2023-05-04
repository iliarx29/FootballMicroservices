import http from 'k6/http';
import { sleep } from 'k6';

export const options = {
    stages: [
        { duration: '5s', target: 5 },
        { duration: '30s', target: 5 },
        { duration: '5s', target: 20 },
        { duration: '30s', target: 20 },
        { duration: '5s', target: 5 },
        { duration: '30s', target: 5 },
        { duration: '5s', target: 0 },
    ],
    thresholds: {
        http_req_duration: ['p(95)<600'],
    },
};
export default function () {
    http.get("https://localhost:7118/api/matches");
    sleep(1);
}