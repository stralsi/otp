import React, {useEffect, useState} from 'react';
import {ProgressBar} from 'baseui/progress-bar';


function percentFromSecondsLeft (secondsLeft, secondsTotal = 30) {
  const diffPercent = secondsLeft/secondsTotal * 100;
      
  return diffPercent > 0 ? diffPercent : 0;
}

export default function ExpirationTimer ({
  expirationDate = new Date(),
  secondsTotal = 30,
}) {
  const [secondsLeft, setSecondsLeft] = useState(30);


  useEffect(() => {
    setTimeout(() => {
      const diffTime = expirationDate - new Date();
      const diffSeconds = diffTime / 1000;
      setSecondsLeft(diffSeconds);
    },100)
  })

  return <ProgressBar
    value={percentFromSecondsLeft(secondsLeft, secondsTotal)}
    showLabel
    getProgressLabel={_ => secondsLeft > 0 ? `valid for ${Math.floor(secondsLeft)}s`: 'Expired'}
    />;
};