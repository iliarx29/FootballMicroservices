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

//export const options = {
//    stages: [
//        { duration: '5s', target: 200 },
//        { duration: '1m', target: 200 },
//        { duration: '5s', target: 0 },
//    ],
//    thresholds: {
//        http_req_duration: ['p(95)<600'],
//    },
//};

//export const options = {
//    stages: [
//        { duration: '10m', target: 16 },
//        { duration: '40m', target: 16 },
//        { duration: '5m', target: 5 },
//        { duration: '1m', target: 0 },
//    ],
//    thresholds: {
//        http_req_duration: ['p(95)<600'],
//    },
//};
export default function () {
    http.get("https://localhost:7118/api/matches/leagues/f3eb147f-924b-499f-83f9-27702172cef6/standings?seasonId=6de7e6c5-d265-4cbe-b81b-12b42b5737fb");
    sleep(1);
}