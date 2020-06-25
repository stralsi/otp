import React, {useEffect, useState} from 'react';
import {ProgressBar} from 'baseui/progress-bar';


function percentFromSecondsLeft (secondsLeft, secondsTotal = 30) {
  const diffPercent = secondsLeft/secondsTotal * 100;
      
  return diffPercent > 0 ? diffPercent : 0;
}

function secondsDiff(date1, date2) {
  const diffTime = date1 - date2;
  return diffTime / 1000;
}

export default function ExpirationTimer ({
  expirationDate = new Date(),
  secondsTotal = 30,
  now = () => new Date(),
}) {
  const [secondsLeft, setSecondsLeft] = useState(secondsDiff(expirationDate, now()));

  useEffect(() => {
    setTimeout(() => {
      setSecondsLeft(secondsDiff(expirationDate, now()))
    },100)
  })

  return <ProgressBar
    value={percentFromSecondsLeft(secondsLeft, secondsTotal)}
    showLabel
    getProgressLabel={_ => secondsLeft > 0 ? `valid for ${Math.floor(secondsLeft)}s`: 'Expired'}
    />;
};